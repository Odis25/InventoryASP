﻿using InventoryAppData.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryAppData
{
    public class DataSeeder
    {
        private InventoryContext _context;

        public DataSeeder(InventoryContext context)
        {
            _context = context;
        }

        public async Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<ApplicationUser>(_context);
            var hasher = new PasswordHasher<ApplicationUser>();

            var user = new ApplicationUser
            {
                UserName = "SuperUser",
                NormalizedUserName = "superuser",
                Email = "artem.budanov@incomsystem.ru",
                NormalizedEmail = "artem.budanov@incomsystem.ru",
                LockoutEnabled = false,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var hashedPassword = hasher.HashPassword(user, "Admin");
            user.PasswordHash = hashedPassword;
           
            var hasAdminRole = _context.Roles.Any(roles => roles.NormalizedName == "admin");
            var hasSuperUser = _context.Users.Any(u => u.NormalizedUserName == user.NormalizedUserName);
            
            // Создаем роль админа, если ее нет
            if (!hasAdminRole)
            {
                await roleStore.CreateAsync(new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "admin"
                });
            }

            //Создаем суперюзера если его еще нет
            if (!hasSuperUser)
            {
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }

            await _context.SaveChangesAsync();
        }

        public async Task SeedData()
        {
            var hasDepartments = _context.Departments.Any();
            var hasPositions = _context.Positions.Any();

            if (!hasDepartments)
            {
                var departments = new Department[]
                {
                    new Department { Name = "ПНРСУ" },
                    new Department { Name = "ТОиС" }
                };

                _context.Departments.AddRange(departments);
                _context.SaveChanges();
            }

            if (!hasPositions)
            {
                var positions = new Position[]
                {
                    new Position { Name = "Инженер-стажер"},
                    new Position { Name = "Инженер"},
                    new Position { Name = "Старший инженер"},
                    new Position { Name = "Ведущий инженер"},
                    new Position { Name = "Зам. начальника отдела"},
                    new Position { Name = "Начальник отдела"}
                };

                _context.AddRange(positions);
                await _context.SaveChangesAsync();
            }
        }
    }
}