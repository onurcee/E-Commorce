﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace iakademi47_proje.Models
{
	public class Cls_Category
	{
		iakademi47Context context = new iakademi47Context();
		public async Task<List<Category>> CategorySelect()
		{
			List<Category> categories = await context.Categories.ToListAsync();
			return categories;
		}

		public List<Category> CategorySelectMain()
		{
			List<Category> categories = context.Categories.Where(c => c.ParentID == 0).ToList();
			return categories;
		}

		public static string CategoryInsert(Category category)
		{
			//metod static olduğunu için

			using (iakademi47Context context = new iakademi47Context())
			{
				try
				{
					Category cat = context.Categories.FirstOrDefault(c => c.CategoryName.ToLower() == category.CategoryName.ToLower());

					if (cat == null)
					{
						context.Add(category);
						context.SaveChanges();
						return "Başarılı";
					}
					else
					{
						return "Bu Kategori Zaten Var!!";
					}
				}

				catch (Exception)
				{
					return "Başarısız!";
				}
			}

		}

		public async Task<Category> CategoryDetails(int? id)
		{
			Category category = await context.Categories.FindAsync(id);
			return category;
		}

		//bool answer = Cls_Category.CategoryUpdate(category);
		public static bool CategoryUpdate(Category category)
		{
			//metod static olduğu için
			using (iakademi47Context context = new iakademi47Context())
			{
				try
				{
					context.Update(category);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;
				}

			}



		}

		public static bool CategoryDelete(int id)
		{
			try
			{
				using (iakademi47Context context = new iakademi47Context())
				{
					Category category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
					category.Active = false;

					List<Category> categories = context.Categories.Where(c => c.ParentID== id).ToList();

					foreach(var item in categories)
					{
					item.Active = false;
					
					}
					context.SaveChanges();
					return true;

				}
			}
			catch (Exception)
			{
				throw;
			
			}


			}

    }
}
