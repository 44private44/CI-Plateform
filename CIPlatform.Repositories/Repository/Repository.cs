
using CIPlatform.Entities.DataModels;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public CiplatformContext _context;

        public Repository(CiplatformContext context)
        {
            _context = context;

        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
