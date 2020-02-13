using InventoryAppData;
using InventoryAppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryAppServices
{
    public class CheckoutService : ICheckout
    {
        private readonly InventoryContext _context;

        public CheckoutService(InventoryContext context)
        {
            _context = context;
        }

        // Получить все записи
        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        // Получить историю записей
        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int deviceId)
        {
            return _context.CheckoutHistories
                .Where(history => history.Device.Id == deviceId);
        }

        // Получить запись по ID
        public Checkout GetById(int checkoutId)
        {
            return _context.Checkouts.Find(checkoutId);
        }

        // Получить последнюю запись по ID устройства
        public Checkout GetLatestCheckout(int deviceId)
        {
            return _context.Checkouts
                .OrderByDescending(c => c.Since)
                .FirstOrDefault(c => c.Device.Id == deviceId);
        }

        // Добавить новую запись
        public void Add(Checkout newCheckout)
        {
            _context.Checkouts.Add(newCheckout);
            _context.SaveChanges();
        }

        // Выдать устройства сотруднику
        public void CheckOutItems(int employeeId, params int[] deviceId)
        {
            if (deviceId.Length == 0)
                return;

            var employee = _context.Employees.Find(employeeId);
            var now = DateTime.Now;

            foreach (var id in deviceId)
            {
                var device = _context.Devices.Find(id);

                // Создаем запись о получении оборудования
                _context.Checkouts.Add(new Checkout
                {
                    Employee = employee,
                    Device = device,
                    Since = now
                });

                // Создаем историю использования
                _context.CheckoutHistories.Add(new CheckoutHistory
                {
                    Employee = employee,
                    Device = device,
                    CheckedOut = now
                });

                // Обновляем статус устройства
                UpdateDeviceStatus(id, "Checked Out");
            }

            _context.SaveChanges();
        }

        // Вернуть устройство
        public void CheckInItem(int deviceId)
        {
            var now = DateTime.Now;

            //Закрываем чекауты
            RemoveExistingCheckouts(deviceId);
            // Закрываем открытую историю
            CloseExistingCheckoutHistory(deviceId, now);
            // Обновляем статус устройства
            UpdateDeviceStatus(deviceId, "Available");

            _context.SaveChanges();
        }

        // Получить ФИО владельца устройства
        public string GetCheckoutHolderFullName(int deviceId)
        {
            var checkout = _context.Checkouts.FirstOrDefault(c => c.Device.Id == deviceId);
            if (checkout != null)
            {
                var holder = checkout.Employee;
                return new StringBuilder()
                    .Append(holder.LastName)
                    .Append(" ")
                    .Append(holder.Name.First())
                    .Append('.')
                    .Append(holder.Patronymic.First())
                    .Append('.')
                    .ToString();
            }
            else return "";
        }

        public int GetCheckoutHolderId(int deviceId)
        {
            return GetLatestCheckout(deviceId)?.Device.Id ?? 0;
        }

        private void UpdateDeviceStatus(int deviceId, string status)
        {
            var device = _context.Devices.Find(deviceId);
            if (device != null)
            {
                _context.Update(device);
                device.Status = status;
            }
        }

        private void CloseExistingCheckoutHistory(int deviceId, DateTime closingTime)
        {
            var history = _context.CheckoutHistories.FirstOrDefault(h => h.Device.Id == deviceId && h.CheckedIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = closingTime;
            }
        }

        private void RemoveExistingCheckouts(int deviceId)
        {
            var checkout = _context.Checkouts
                .FirstOrDefault(checkout => checkout.Device.Id == deviceId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        
    }
}
