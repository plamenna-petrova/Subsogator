﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataAccess.Seeding
{
    public interface ISeeder
    {
        void SeedDatabase(ApplicationDbContext applicationDbContext);
    }
}
