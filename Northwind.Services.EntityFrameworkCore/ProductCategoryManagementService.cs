﻿using Northwind.Services;
using Northwind.Services.Models;

namespace Northwind.Services.EntityFrameworkCore
{
    /// <inheritdoc/>
    public class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private static NorthwindContext? _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryManagementService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ProductCategoryManagementService(NorthwindContext context)
            => _context = context;

        /// <inheritdoc/>
        public async Task<int> CreateCategoryAsync(ProductCategory productCategory)
        {
            _ = productCategory is null ? throw new ArgumentNullException($"{nameof(productCategory)} is null") : productCategory;

            _context.Categories.Add(productCategory);
            await _context.SaveChangesAsync();

            return productCategory.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyCategoryAsync(int categoryId)
        {
            var category = _context.Categories.Find(categoryId);

            if (category is null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<IList<ProductCategory>> ShowCategoriesAsync(int offset, int limit)
        {
            var list = limit != -1 ? _context.Categories.Skip(offset).Take(limit).ToList() : _context.Categories.Skip(offset).ToList();
            
            return list;
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            productCategory = _context.Categories.Find(categoryId);

            if (productCategory is null)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateCategoriesAsync(int categoryId, ProductCategory productCategory)
        {
            var category = _context.Categories
                .Where(cat => cat.Id == categoryId)
                .FirstOrDefault();

            category.Name = productCategory.Name;
            category.Description = productCategory.Description;

            await _context.SaveChangesAsync();

            if (_context.Categories.Contains(productCategory))
            {
                return true;
            }

            return false;
        }
    }
}
