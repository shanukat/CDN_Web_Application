using Freelancers.Models.Domain;
using Freelancers.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freelancers.Controllers
{
    public class FreelancerController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger; //implementing log4net....
        public FreelancerController(IUserService userService, ILogger<FreelancerController> logger) 
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //----- Get Access Token ----
            _logger.LogInformation("-----Program started-----");
            _logger.LogInformation("Get Access Token");

            try
            {
                var token_response = await _userService.GetAccessToken();
                var token = token_response.Token.ToString();
                TempData["AccessToken"] = token;

                var freelancerList = await _userService.GetFreelancers(token);
                return View(freelancerList);
            }
            catch (Exception ex)
            {

                throw new BadHttpRequestException("Bad Request", 400);
                
            }

           // return View();
        }



       
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FreelancerViewModel newFreelancer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _userService.AddFreelancer(newFreelancer, TempData["AccessToken"].ToString());
                    TempData["SuccessMsg"] = "Freelancer (" + newFreelancer.Username + ") added successfully.";

                    _logger.LogInformation("Submit Data successfully");

                    return RedirectToAction(nameof(Index));
                }


                return View(newFreelancer);
            }
            catch
            {
                return View(newFreelancer);
            }
        }

        // GET: FreelancerController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var exist_model = await _userService.GetFreelancerById(id, TempData["AccessToken"].ToString());
            if (exist_model == null)
            {
                _logger.LogError("Data Not Found");
                return NotFound();
            }

            return View(exist_model);
        }

        // POST: FreelancerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FreelancerViewModel updateFreelancer)
        {
            try
            {
                if (id != updateFreelancer.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var update_data = await _userService.UpdateFreelancer(id, updateFreelancer);
                    if (update_data != null)
                    {
                        TempData["SuccessMsg"] = "Freelancer updated successfully.";
                    }
                    else
                    {
                        TempData["ErrorMsg"] = "Error updating existing freelancer record";
                    }
                    
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {
                return View(updateFreelancer);
            }
            return View(updateFreelancer);
        }

        // GET: FreelancerController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var check_freelancer =  await _userService.GetFreelancerById(id, TempData["AccessToken"].ToString());
            if (check_freelancer == null)
            {
                return NotFound();
            }

            return View(check_freelancer);
        }

        // POST: FreelancerController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                string delete_data =  await _userService.DeleteFreelancer(id);
                if (delete_data != null)
                {
                    TempData["SuccessMsg"] = "Freelancer Deleted successfully.";
                }
                else
                {
                    TempData["ErrorMsg"] = "Freelancer not found";
                }
                    return RedirectToAction(nameof(Index));
               
            }
            catch
            {
                
            }
            return View();

        }
    }
}
