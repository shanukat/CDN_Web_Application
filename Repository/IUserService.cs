using Freelancers.Models.Domain;

namespace Freelancers.Repository
{
    public interface IUserService
    {
        Task<TokenResponse> GetAccessToken();
        Task<IEnumerable<FreelancerViewModel>> GetFreelancers(string token);

        Task<FreelancerViewModel> AddFreelancer(FreelancerViewModel newFreelancer, string token);
        Task<string> UpdateFreelancer(int id, FreelancerViewModel updateFreelancer);
        Task<string> DeleteFreelancer(int id);
        Task<FreelancerViewModel> GetFreelancerById(int id, string token);

    }




}
