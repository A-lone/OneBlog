using OneBlog.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneBlog.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
namespace OneBlog.Data
{
    public class CategoriesRepository : BaseRepository, ICategoriesRepository
    {

        private ApplicationContext _ctx;

        public CategoriesRepository(IConfigurationRoot config, ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public CategoryItem Add(CategoryItem item)
        {
            var cat = (from c in _ctx.Categories.ToList() where c.Title == item.Title select c).FirstOrDefault();
            if (cat != null)
            {
                throw new Exception("Category Name must be unique");
            }
            try
            {
                var newItem = new Categories();
                newItem.Title = item.Title;
                newItem.Description = item.Description;
                if (item.Parent == null || string.IsNullOrEmpty(item.Parent.OptionValue))
                {
                    //newItem.ParentId ;
                }
                else
                {
                    var pId = Guid.Parse(item.Parent.OptionValue);
                    newItem.ParentId = pId;
                }
                _ctx.Add(newItem);
                SaveAll();

                item.Id = newItem.Id;
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void SaveAll()
        {
            _ctx.SaveChanges();
        }

        private SelectOption OptionById(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return null;

            var cat = (from c in _ctx.Categories.ToList() where c.Id == id select c).FirstOrDefault();
            return new SelectOption { IsSelected = false, OptionName = cat.Title, OptionValue = cat.Id.ToString() };
        }

        public IEnumerable<CategoryItem> Find(int take = 10, int skip = 0, string filter = "", string order = "")
        {
            // get post categories with counts
            var items = new List<CategoryItem>();
            var categories = _ctx.Categories.Include(m => m.PostsInCategories).ToList();
            // add categories without posts
            foreach (var c in categories)
            {
                items.Add(new CategoryItem { Id = c.Id, Parent = OptionById(c.ParentId), Title = c.Title, Description = c.Description, Count = c.PostsInCategories.Count });
            }

            // return only what requested
            var query = items.AsQueryable();

            //if (!string.IsNullOrEmpty(filter))
            //{
            //    query = items.AsQueryable().Where(filter);
            //}

            if (take == 0)
            {
                take = items.Count;
            }

            if (string.IsNullOrEmpty(order))
                order = "Title";

            return query.Skip(skip).Take(take);
        }

        public CategoryItem FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Guid id)
        {
            try
            {
                var releation = _ctx.PostsInCategories.Where(m => m.CategoriesId == id);

                if (releation != null)
                {
                    foreach (var item in releation)
                    {
                        _ctx.PostsInCategories.Remove(item);
                    }
                }
                var category = _ctx.Categories.FirstOrDefault(m => m.Id == id);
                if (category != null)
                {
                    _ctx.Categories.Remove(category);
                }

                SaveAll();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(CategoryItem item)
        {
            throw new NotImplementedException();
        }

        public List<CategoryItem> GetAll()
        {
            var categories = _ctx.Categories.ToList();
            var list = new List<CategoryItem>();
            foreach (var c in categories)
            {
                int count = _ctx.PostsInCategories.Where(m => m.CategoriesId == c.Id).Count();
                list.Add(new CategoryItem { Id = c.Id, Parent = OptionById(c.ParentId), Title = c.Title, Description = c.Description, Count = count });
            }
            return list;
        }
    }
}
