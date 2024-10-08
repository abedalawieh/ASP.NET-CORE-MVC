﻿using MVCProject.DataAccess.Data;
using MVCProject.DataAccess.Repository.IRepository;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Category = new CategoryRespository(_db);
            Product = new ProductRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

