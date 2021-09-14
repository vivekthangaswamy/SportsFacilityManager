using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.Data
{
    public class Facilityservices : IFacilityservice
    {
        private readonly ApplicationDbContext context;

        public Facilityservices(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds Facility to the database
        /// </summary>
        /// <param name="Facility">New Facility object</param>
        /// <returns>Task representing the operation</returns>
        public async Task AddFacility(Facility Facility)
        {
            await context.Facilitys.AddAsync(Facility);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds all Facilitys that have the searched capacity
        /// </summary>
        /// <typeparam name="T">Data class to map Facility data to</typeparam>
        /// <param name="capacity">The searched Facility capacity</param>
        /// <returns>Task with the Facilitys data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllByCapacity<T>(int capacity)
        {
            return await context.Facilitys.Where(x => x.Capacity == capacity).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all Facilitys that have the searched type
        /// </summary>
        /// <typeparam name="T">Data class to map Facility data to</typeparam>
        /// <param name="type">The searched Facility type</param>
        /// <returns>Task with the Facilitys data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllByType<T>(FacilityType type)
        {
            return await context.Facilitys.Where(x => x.Type == type).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all Facilitys that are available at present
        /// </summary>
        /// <typeparam name="T">Data class to map Facility data to</typeparam>
        /// <returns>Task with the Facilitys data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllFreeFacilitysAtPresent<T>()
        {
            return await context.Facilitys.
                                 Where(x => !x.Reservations.Any(r => r.AccommodationDate <= DateTime.Today &&
                                                                     r.ReleaseDate > DateTime.Today)).
                                 OrderBy(x => x.Number).
                                 ProjectTo<T>().
                                 ToListAsync();
        }

        /// <summary>
        /// Finds all Facilitys that are available at present
        /// </summary>
        /// <returns>Task with the count of the Facilitys data that satisfy the criteria parsed to 
        /// <typeparamref name="T"/> object or null if not found</returns>
        public async Task<int> CountFreeFacilitysAtPresent()
        {
            return await context.Facilitys.
                                 Where(x => !x.Reservations.Any(r => r.AccommodationDate <= DateTime.Today 
                                                                  && r.ReleaseDate > DateTime.Today)).
                                 CountAsync();
        }

        /// <summary>
        /// Finds all Facilitys in the database
        /// </summary>
        /// <typeparam name="T">Data class to map Facility data to</typeparam>
        /// <returns>Task with all Facilitys data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await context.Facilitys.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all Facilitys that satisfy the criteria
        /// </summary>
        /// <typeparam name="T">Data class to map Facility data to</typeparam>
        /// <param name="availableOnly">Indicate if the Facilitys should be available</param>
        /// <param name="types">The filtered Facilitys type<sparam>
        /// <param name="minCapacity">The min capacity of the filtered Facilitys</param>
        /// <returns>Task with filtered Facilitys data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetSearchResults<T>(bool availableOnly = false, 
                                                              FacilityType[] types = null, 
                                                              int? minCapacity = null)
        {
            IQueryable<Facility> result = context.Facilitys;

            if (availableOnly)
            {
                result = result.Where(x => !x.Reservations.Any(r => r.AccommodationDate <= DateTime.Today
                                                                 && r.ReleaseDate > DateTime.Today));
            }

            if (types != null && (types?.Count() ?? 0) > 0)
            {
                result = result.Where(x => types.Contains(x.Type));
            }

            if (minCapacity != null && minCapacity > 0)
            {
                result = result.Where(x => x.Capacity > minCapacity);
            }

            return await result.OrderBy(x => x.Number).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Removes Facility from the database
        /// </summary>
        /// <param name="id">The Facility to delete id</param>
        /// <returns>Task representing the operation</returns>
        public async Task DeleteFacility(string id)
        {
            var Facility = await context.Facilitys.Include(x => x.Reservations).FirstOrDefaultAsync(x => x.Id == id);
            if (Facility != null)
            {
                //Feature: Send an email for Facility cancel forced
                if (Facility.Reservations != null)
                {
                    context.Reservations.RemoveRange(Facility.Reservations);
                    await context.SaveChangesAsync();
                }

                context.Facilitys.Remove(Facility);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the data of existing Facility
        /// </summary>
        /// <param name="id">Existing Facility id</param>
        /// <param name="Facility">Facility data to change with</param>
        /// <returns>Task representing the operation</returns>
        public async Task UpdateFacility(string id, Facility Facility)
        {
            Facility.Id = id;
            var FacilityToChange = await context.Facilitys.AsNoTracking().Include(x=>x.Reservations).FirstOrDefaultAsync(x=>x.Id==id);
            if (FacilityToChange != null)
            {
                if (FacilityToChange.Reservations != null)
                {
                    foreach (var reservation in FacilityToChange.Reservations)
                    {
                        if (FacilityToChange.Capacity < Facility.Capacity)
                        {
                            //Feature: Send an email for change & not cancel till confirmation
                            context.Reservations.Remove(reservation);
                        }
                    }
                }

                context.Facilitys.Update(Facility);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Finds the searched Facility
        /// </summary>
        /// <typeparam name="T">Data class to map Facility data to</typeparam>
        /// <param name="id">Searched Facility id</param>
        /// <returns>Task with the Facility data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<T> GetFacility<T>(string id)
        {
            return await this.context.Facilitys.Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        ///<returns>The count of all Facilitys in the database</returns>
        public int CountAllFacilitys()
        {
            return context.Facilitys.Count();
        }

        /// <summary>
        /// Finds the lowest adult price of the Facilitys in the database
        /// </summary>
        /// <returns>Task with the minimum price result</returns>
        public async Task<double> GetMinPrice()
        {
            return await this.context.Facilitys.OrderBy(x => x.AdultPrice).Select(X => X.AdultPrice).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds the highest adult price of the Facilitys in the database
        /// </summary>
        /// <returns>Task with the maximum price result</returns>
        public async Task<double> GetMaxPrice()
        {
            return await this.context.Facilitys.
                                      OrderByDescending(x => x.AdultPrice).
                                      Select(X => X.AdultPrice).
                                      FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds if a Facility is free
        /// </summary>
        /// <param name="number">The searched Facility number</param>
        /// <param name="FacilityId">The Facility numer to update, to exclude its Number from the search</param>
        /// <returns>Task with the Facility numer availability result</returns>
        public async Task<bool> IsFacilityNumberFree(int number, string FacilityId = null)
        {
            return !await context.Facilitys.AsNoTracking().Where(x => x.Id != FacilityId).AnyAsync(x => x.Number == number);
        }

        /// <summary>
        /// Finds the highest capacity of the Facilitys in the database
        /// </summary>
        /// <returns>Task with the maximum Facility capacity result</returns>
        public async Task<int> GetMaxCapacity()
        {
            return await context.Facilitys.AsNoTracking().
                                       OrderByDescending(x => x.Capacity).
                                       Select(x => x.Capacity).
                                       FirstOrDefaultAsync();
        }
    }
}
