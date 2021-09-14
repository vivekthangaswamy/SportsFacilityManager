using Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Enums;

namespace Services.Data
{
    public interface IFacilityservice
    {
        public Task AddFacility(Facility Facility);
        public Task<IEnumerable<T>> GetAllByCapacity<T>(int capacity);
        public Task<IEnumerable<T>> GetAllByType<T>(FacilityType type);
        public Task<IEnumerable<T>> GetAllFreeFacilitysAtPresent<T>();
        public Task<int> CountFreeFacilitysAtPresent();
        public Task<IEnumerable<T>> GetAll<T>();
        public Task<IEnumerable<T>> GetSearchResults<T>(bool availableOnly = false,
                                                        FacilityType[] types = null,
                                                        int? minCapacity = null);
        public Task DeleteFacility(string id);
        public Task UpdateFacility(string id, Facility Facility);
        public Task<T> GetFacility<T>(string id);
        public int CountAllFacilitys();
        public Task<double> GetMinPrice();
        public Task<double> GetMaxPrice();
        public Task<bool> IsFacilityNumberFree(int number, string FacilityId = null);
        public Task<int> GetMaxCapacity();
    }

}
