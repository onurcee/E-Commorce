using Microsoft.EntityFrameworkCore;

namespace iakademi47_proje.Models
{
    public class Cls_Status
    {
        iakademi47Context context = new iakademi47Context();
        public async Task<List<Status>> StatusSelect()
        {
            List<Status> stasuses = await context.Statuses.ToListAsync();
            return stasuses;
        }

        public static string StatusInsert(Status status)
        {
            //metod static olduğunu için

            using (iakademi47Context context = new iakademi47Context())
            {
                try
                {
                    Status st = context.Statuses.FirstOrDefault(s => s.StatuName.ToLower() == status.StatuName.ToLower());

                    if (st == null)
                    {
                        context.Add(status);
                        context.SaveChanges();
                        return "Başarılı";
                    }
                    else
                    {
                        return "Bu Statü Zaten Var!!";
                    }
                }

                catch (Exception)
                {
                    return "Başarısız!";
                }
            }

        }

		public async Task<Status?> StatusDetails(int? id)
		{
			Status? statuses = await context.Statuses.FindAsync(id);
			return statuses;
		}

		public static bool StatusUpdate(Status status)
		{
			//metod static olduğu için
			using (iakademi47Context context = new iakademi47Context())
			{
				try
				{

					context.Update(status);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;
				}

			}


		}

		public static bool StatusDelete(int id)
		{
			try
			{
				using (iakademi47Context context = new iakademi47Context())
				{
					Status status = context.Statuses.FirstOrDefault(st => st.StatusID == id);
					status.Active = false;

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
