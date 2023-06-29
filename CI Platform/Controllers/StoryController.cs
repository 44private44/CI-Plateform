using CIPlatform.Entities.DataModels;
using CIPlatform.Entities.MissionModel;
using CIPlatform.Repositories.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace CI_Platform.Controllers
{

    public class StoryController : Controller
    {
        private CiplatformContext _context;
        private readonly ILogger<StoryController> _logger;
        private readonly IStoryListingRepository _storyListingRepository;
        private readonly IShareStoryRepository _shareStoryRepository;
        private readonly IStoryDetailsRepository _storyDetailsRepository;
        public StoryController(CiplatformContext context, ILogger<StoryController> logger, IStoryListingRepository storyListingRepository, IShareStoryRepository shareStoryRepository, IStoryDetailsRepository storyDetailsRepository)
        {
            _context = context;
            _logger = logger;
            _storyListingRepository = storyListingRepository;
            _shareStoryRepository = shareStoryRepository;
            _storyDetailsRepository = storyDetailsRepository;
        }
        public IActionResult Storylisting(int page)
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);
            ViewBag.UserId = UserId;
            var totalStories = _context.Stories.Where(storystatus => storystatus.Status == "PUBLISHED")?.Count();

            var pageNumber = 0;

            if (page != 0)
            {

                pageNumber = page;
            }
            else
            {
                pageNumber = 1;
            }
            var pageSize = 3;
            ViewBag.TotalPages = Math.Ceiling((double)totalStories / pageSize);
            var skipRows = (pageNumber - 1) * pageSize;
            StorylistingModel storydetails = _storyListingRepository.GetAllStoryDetails(UserId, skipRows, pageSize);

            ViewBag.PageNumber = pageNumber;

            return View(storydetails);

        }

        public IActionResult Sharestory()
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            SharestoryModel sharestorydetails = _shareStoryRepository.GetAllShareStoryData(UserId);
            return View(sharestorydetails);

        }

        [HttpPost]
        public IActionResult Storydatasave(SharestoryModel formData)
        {
            // int missionId, String title, DateTime date, String discription, string videourl, string storytype, string firstName, List<IFormFile> imageNames
            //string[] imageNamesArray = imageNames.Split(',');
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var existingStory = _context.Stories.SingleOrDefault(sm => sm.MissionId == formData.MissionId && sm.UserId == UserId);

            if (existingStory == null)
            {
                var newstory = new Story
                {
                    UserId = UserId,
                    MissionId = formData.MissionId,
                    Title = formData.StoryTitle,
                    Description = formData.Description,
                    Status = "DRAFT",
                    PublishedAt = formData.StoryDate,
                    Views = 0,
                };
                _context.Stories.Add(newstory);
                _context.SaveChanges();

                foreach (var image in formData.Images)
                {
                    if (image != null)
                    {
                        var imageename = image.FileName;
                        var guid = Guid.NewGuid().ToString().Substring(0, 8);
                        var fileName = $"{UserId}_ {formData.MissionId}_{guid}_{imageename}"; // getting filename

                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IMAGES", "CI Platform Assets", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                        var newMedium = new StoryMedium
                        {
                            StoryId = newstory.StoryId,
                            Path = $"IMAGES/CI Platform Assets/{fileName}",
                            Type = formData.Type,
                            VideoUrl = formData.VideoURLs,
                        };
                        _context.StoryMedia.Add(newMedium);
                        _context.SaveChanges();
                    }
                }
            }
            else
            {
                existingStory.Title = formData.StoryTitle;
                existingStory.Description = formData.Description;
                existingStory.Status = "DRAFT";
                existingStory.PublishedAt = formData.StoryDate;

                // find the existing media for the story
                var existingMediaList = _context.StoryMedia.Where(m => m.StoryId == existingStory.StoryId).ToList();

                _context.StoryMedia.RemoveRange(existingMediaList);

               var getstoryid = _context.Stories.Where(storyid => storyid.UserId == UserId && storyid.MissionId == formData.MissionId).Select(storyid => storyid.StoryId).FirstOrDefault(); 

                _context.SaveChanges();
                foreach (var image in formData.Images)
                {
                    if (image != null)
                    {
                        var imageename = image.FileName;
                        var guid = Guid.NewGuid().ToString().Substring(0, 8);
                        var fileName = $"{UserId}_ {formData.MissionId}_{guid}_{imageename}"; // getting filename

                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IMAGES", "CI Platform Assets", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(fileStream);
                        }
                        var newMedium = new StoryMedium
                        {
                            StoryId = getstoryid,
                            Path = $"IMAGES/CI Platform Assets/{fileName}",
                            Type = formData.Type,
                            VideoUrl = formData.VideoURLs,
                        };
                        _context.StoryMedia.Add(newMedium);
                        _context.SaveChanges();
                    }
                }

                _context.SaveChanges();

            }

            return Json(new { success = true });
        }
        // submit story 
        [HttpPost]
        public IActionResult Storydata(int missionId, String title, DateTime date, String discription, string videourl, string storytype) // , string firstName , string imageNames
        {
            //string[] imageNamesArray = imageNames.Split(',');
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var existingStory = _context.Stories.SingleOrDefault(sm => sm.MissionId == missionId && sm.UserId == UserId);


            if (existingStory == null)
            {
                var newstory = new Story
                {
                    UserId = UserId,
                    MissionId = missionId,
                    Title = title,
                    Description = discription,
                    Status = "PUBLISHED",
                    PublishedAt = date
                };
                _context.Stories.Add(newstory);
                _context.SaveChanges();

                //for (int i = 0; i < imageNamesArray.Length; i++)
                //{
                //    var newMedium = new StoryMedium
                //    {
                //        StoryId = newstory.StoryId,
                //        Path = $"IMAGES/CI Platform Assets/{imageNamesArray[i]}",
                //        Type = storytype,
                //        VideoUrl = videourl
                //    };
                //    _context.StoryMedia.Add(newMedium);
                //    _context.SaveChanges();
                //}
            }
            else
            {
                existingStory.Title = title;
                existingStory.Description = discription;
                existingStory.Status = "PUBLISHED";
                existingStory.PublishedAt = date;

                // find the existing media for the story
                var existingMediaList = _context.StoryMedia.Where(m => m.StoryId == existingStory.StoryId).ToList();

                //for (int i = 0; i < imageNamesArray.Length; i++)
                //{
                //    if (i < existingMediaList.Count)
                //    {
                //        // update existing media record
                //        existingMediaList[i].Path = $"IMAGES/CI Platform Assets/{imageNamesArray[i]}";
                //        existingMediaList[i].Type = storytype;
                //        existingMediaList[i].VideoUrl = videourl;
                //    }
                //    else
                //    {
                //        // create a new media record for the new image
                //        var newMedium = new StoryMedium
                //        {
                //            StoryId = existingStory.StoryId,
                //            Path = $"IMAGES/CI Platform Assets/{imageNamesArray[i]}",
                //            Type = storytype,
                //            VideoUrl = videourl
                //        };
                //        _context.StoryMedia.Add(newMedium);
                //    }

                //}

                _context.SaveChanges();

            }

            return Json(new { success = true });
        }

        // buttonstatus
        [HttpPost]

        public IActionResult Buttonstatus(int selectedMissionId)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            // Retrieve the record(s) from the story table based on the provided IDs
            var storyavailable = _context.Stories
                .Where(s => s.MissionId == selectedMissionId && s.UserId == UserId)
                .FirstOrDefault();

            if (storyavailable != null)
            {
                // Check the status field
                if (storyavailable.Status == "PUBLISHED")
                {
                    // Status is published
                    return Json("published");
                }
                else
                {
                    // Status is not published
                    return Json("draft");
                }
            }
            else
            {
                // Record not found
                return NotFound();
            }
        }

        //storydetails
        public IActionResult Storydetails(int id)
        {
            if (HttpContext.Session.GetString("Token") == "null")
            {
                return RedirectToAction("Login", "UserAuth");
            }

            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            ViewBag.userId = UserId;
            StorydetailsModel sharestorydetails = _storyDetailsRepository.GetOneStoryData(UserId, id);
            return View(sharestorydetails);

        }

        //recommended co-worker
        [HttpPost]
        public IActionResult recommendedcoworkers(string recoemail, int recomstoryid)
        {
            if (ModelState.IsValid)
            {
                string Id2 = HttpContext.Session.GetString("userId");
                int UserId2 = int.Parse(Id2);

                var obj2 = _context.Users.Where(a => a.Email.Equals(recoemail)).FirstOrDefault();
                var touserid = _context.Users.Where(id => id.Email == recoemail).Select(id => id.UserId).FirstOrDefault();
                var recocheckvalidate = _context.StoryInvites.Where(reco => reco.StoryId == recomstoryid && reco.FromUserId == UserId2
                                            && reco.ToUserId == touserid).Select(reco => reco.StoryInviteId).FirstOrDefault();

                if (obj2 != null)
                {
                    if (recocheckvalidate == 0)
                    {
                        //link generate
                        var PasswordResetLink = Url.Action("Login", "UserAuth", new { Email = recoemail }, Request.Scheme);

                        // retrieve the user_id based on recoemail parameter
                        string Id = HttpContext.Session.GetString("userId");
                        int UserId = int.Parse(Id);

                        long toUserId = _context.Users.Where(a => a.Email.Equals(recoemail)).Select(a => a.UserId).FirstOrDefault();

                        //store data into table 
                        var storyrecoinvite = new StoryInvite()
                        {
                            FromUserId = UserId,
                            ToUserId = toUserId,
                            StoryId = recomstoryid

                        };
                        _context.StoryInvites.Add(storyrecoinvite);
                        _context.SaveChanges();


                        //email sent
                        var fromEmail = new MailAddress("sohammodi124421@gmail.com");
                        var toEmail = new MailAddress(recoemail);
                        var fromEmailPassword = "whwuvzwoegqezftj";
                        string subject = "Recommende Co-worker";
                        string body = PasswordResetLink;


                        var smtp = new SmtpClient
                        {

                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                        };

                        MailMessage message = new MailMessage(fromEmail, toEmail);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }

                }
                else
                {
                    ModelState.AddModelError("Email", "Email is not Registered");
                }

            }
            return View(recoemail);
        }


        // Draft data
        [HttpPost]

        public IActionResult Draftdatashow(int choosemissionid)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            var draftStory = _context.Stories.Include(s => s.StoryMedia)
                               .FirstOrDefault(s => s.MissionId == choosemissionid && s.UserId == UserId && s.Status == "DRAFT");

            var draftstoryid = _context.Stories.Where(image => image.UserId == UserId && image.MissionId == choosemissionid && image.Status == "DRAFT").
                                    Select(image => image.StoryId).FirstOrDefault();
            var drftimages = _context.StoryMedia.Where(image => image.StoryId == draftstoryid).Select(image => image.Path).ToList();
            if (draftStory != null)
            {
                var data = new
                {
                    title = draftStory.Title,
                    description = draftStory.Description,
                    date = draftStory.PublishedAt,
                    status = draftStory.StoryMedia.FirstOrDefault().Type,
                    url = draftStory.StoryMedia.FirstOrDefault()?.VideoUrl,
                    storydraftimages = drftimages
                };
                return Json(new { success = true, data = data });
            }
            else
            {
                return Json(new { success = false });
            }


        }


        //Previewdata
        public IActionResult previewdata(int id)
        {
            string Id = HttpContext.Session.GetString("userId");
            int UserId = int.Parse(Id);

            int storyId = (int)_context.Stories.Where(s => s.MissionId == id && s.UserId == UserId).Select(s => s.StoryId).FirstOrDefault();

            return RedirectToAction("Storydetails", "Story", new { id = storyId });

        }
    }
}
