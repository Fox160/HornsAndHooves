﻿using Service.BindingModels;
using Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IProductService
    {
        List<ProductViewModel> GetList();

        ProductViewModel GetElement(int id);

        void AddElement(ProductBindingModel model);

        void UpdElement(ProductBindingModel model);

        void DelElement(int id);
    }
}
