﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIPlatform.Repositories.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        void Add(T entity);
        void Save();


    }
}
