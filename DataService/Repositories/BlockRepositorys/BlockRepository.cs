﻿using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.BlockRepositorys
{
    public class BlockRepository : Repository<Block>, IBlockRepository
    {
        private readonly TransferCompanyContext _context;
        public BlockRepository(TransferCompanyContext context):base(context)
        {
            _context = context;
        }
        public void update(Block block)
        {
            _context.Update(block);
        }
    }
}
