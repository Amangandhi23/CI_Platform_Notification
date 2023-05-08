using CI_Platform.Entities.Data;
using CI_Platform.Entities.Models;
using CI_Platform.Entities.ViewModel;
using CI_Platform.Repository.Interface;
using Microsoft.EntityFrameworkCore;


namespace CI_Platform.Repository.Repository
{
    public class MissiondataRepository : IMissiondataRepository
    {
        private readonly CiPlatformContext _db;
        private ICountryRepository _countryname;
        private ICityRepository _cityname;
        private ISkillRepository _skillname;
        private IThemeRepository _themename;
        private IMissionRepository _mission;


        public MissiondataRepository(CiPlatformContext db,ICountryRepository countryname, ICityRepository cityname, ISkillRepository skillname, IThemeRepository themename, IMissionRepository mission)
        {
            _db = db;
            _countryname = countryname;
            _cityname = cityname;
            _skillname = skillname;
            _themename = themename;
            _mission = mission;
        }

        public Mission_data GetMissiondata()
        {
            Mission_data missiondata = new();

            List<MissionMedium> missionMedia = _db.MissionMedia.ToList();
            missiondata.missionMedia = missionMedia;

            List<Country> country = _countryname.GetCountryData();
            missiondata.Country = country;

            List<City> city = _cityname.GetCityData();
            missiondata.City = city;

            List<Skill> skill = _skillname.GetSkillData();
            missiondata.Skill = skill.Where(skillid => skillid.DeletedAt == null && skillid.Status == 1);

            List<MissionTheme> theme = _themename.GetThemeData();
            missiondata.MissionTheme = theme.Where(themeid => themeid.DeletedAt == null && themeid.Status == 1); ;

            List<Comment> commment = _db.Comments.ToList();
            missiondata.comments = commment;

            List<MissionRating> missionRatings = _db.MissionRatings.ToList();
            missiondata.MissionRating = missionRatings;

            List<Mission> mission = _mission.GetMissionData().Where(mission => mission.DeletedAt == null && mission.Status == "1" && mission.Theme.DeletedAt == null).ToList();
            missiondata.Mission = mission;

            

            List<FavoriteMission> fm = _db.FavoriteMissions.ToList();
            missiondata.FavoriteMission = fm;

            List<MissionApplication> ma = _db.MissionApplications.ToList();
            missiondata.MissionApplication = ma;

            return missiondata;
        }


        public IEnumerable<Mission> ApplyFilter(string[] country, string[] city, string[] skill, string[] theme, string sort, long userid, string search)
        {
            Mission_data missionobj = GetMissiondata();
            IEnumerable<Mission> missions = missionobj.Mission;
            IEnumerable<Mission> filterMissions = new List<Mission>();

            if (country.Length != 0 && city.Length != 0 && theme.Length != 0 && skill.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && city.Contains(m.City.Name) && theme.Contains(m.Theme.Title) && m.MissionSkills.Any(s => skill.Contains(s.Skill.SkillName)));
                foreach (string countryname in country)
                {
                    foreach (string cityname in city)
                    {
                        foreach (string themename in theme)
                        {
                            foreach (string skillname in skill)
                            {
                                missionsList = missionsList.Where(m => m.Country.Name == countryname && m.Theme.Title == themename && m.City.Name == cityname && m.MissionSkills.Any(s => s.Skill.SkillName == skillname));
                            }
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }
            }

            else if (country.Length != 0 && city.Length != 0 && theme.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && city.Contains(m.City.Name) && theme.Contains(m.Theme.Title));
                foreach (string countryname in country)
                {
                    foreach (string cityname in city)
                    {
                        foreach (string themename in theme)
                        {
                            missionsList = missionsList.Where(m => m.Country.Name == countryname && m.Theme.Title == themename && m.City.Name == cityname);
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }
            }

            else if (country.Length != 0 && theme.Length != 0 && skill.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && m.MissionSkills.Any(s => skill.Contains(s.Skill.SkillName)) && theme.Contains(m.Theme.Title));
                foreach (string countryname in country)
                {
                    foreach (string themename in theme)
                    {
                        foreach (string skillname in skill)
                        {
                            missionsList = missionsList.Where(m => m.Country.Name == countryname && m.Theme.Title == themename && m.MissionSkills.Any(s => s.Skill.SkillName == skillname));
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }

            }

            else if (country.Length != 0 && city.Length != 0 && skill.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && m.MissionSkills.Any(s => skill.Contains(s.Skill.SkillName)) && city.Contains(m.City.Name));
                foreach (string countryname in country)
                {
                    foreach (string cityname in city)
                    {
                        foreach (string skillname in skill)
                        {
                            missionsList = missionsList.Where(m => m.Country.Name == countryname && m.City.Name == cityname && m.MissionSkills.Any(s => s.Skill.SkillName == skillname));
                        }
                        filterMissions = filterMissions.Concat(missionsList);
                    }
                }

            }

            else if (city.Length != 0 && theme.Length != 0 && skill.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.City.Name == city[0] && theme.Contains(m.Theme.Title) && m.MissionSkills.Any(s => skill.Contains(s.Skill.SkillName)));
                foreach (string cityname in city)
                {
                    foreach (string themename in theme)
                    {
                        foreach (string skillname in skill)
                        {
                            missionsList = missionsList.Where(m => m.Theme.Title == themename && m.City.Name == cityname && m.MissionSkills.Any(s => s.Skill.SkillName == skillname));
                        }
                    }
                    filterMissions = filterMissions.Concat(missionsList);
                }
            }

            else if (city.Length != 0 && theme.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.City.Name == city[0] && theme.Contains(m.Theme.Title));
                foreach (string cityname in city)
                {
                    foreach (string themename in theme)
                    {
                        missionsList = missionsList.Where(m => m.Theme.Title == themename && m.City.Name == cityname);
                    }
                }
                filterMissions = filterMissions.Concat(missionsList);
            }

            else if (theme.Length != 0 && skill.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.MissionSkills.Any(s => s.Skill.SkillName == skill[0]) && theme.Contains(m.Theme.Title));
                foreach (string themename in theme)
                {
                    foreach (string skillname in skill)
                    {
                        missionsList = missionsList.Where(m => m.Theme.Title == themename && m.MissionSkills.Any(s => s.Skill.SkillName == skillname));
                    }
                }
                filterMissions = filterMissions.Concat(missionsList);
            }

            else if (city.Length != 0 && skill.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.MissionSkills.Any(s => skill.Contains(s.Skill.SkillName)) && m.City.Name == city[0]);
                foreach (string cityname in city)
                {
                    foreach (string skillname in skill)
                    {
                        missionsList = missionsList.Where(m => m.City.Name == cityname && m.MissionSkills.Any(s => s.Skill.SkillName == skillname));
                    }
                }
                filterMissions = filterMissions.Concat(missionsList);
            }

            else if (city.Length != 0 && country.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && city.Contains(m.City.Name));
                foreach (string countryname in country)
                {
                    foreach (string cityname in city)
                    {
                        missionsList = missionsList.Where(m => m.City.Name == cityname && m.Country.Name == countryname);

                    }
                }
                filterMissions = filterMissions.Concat(missionsList);
            }

            else if (theme.Length != 0 && country.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && theme.Contains(m.Theme.Title));
                foreach (string countryname in country)
                {
                    foreach (string themename in theme)
                    {
                        missionsList = missionsList.Where(m => m.Theme.Title == themename && m.Country.Name == countryname);
                    }
                }
                filterMissions = filterMissions.Concat(missionsList);

            }

            else if (skill.Length != 0 && country.Length != 0)
            {
                IEnumerable<Mission> missionsList = new List<Mission>();
                filterMissions = missions.Where(m => m.Country.Name == country[0] && m.MissionSkills.Any(s => skill.Contains(s.Skill.SkillName)));
                foreach (string countryname in country)
                {
                    foreach (string skillname in skill)
                    {
                        missionsList = missionsList.Where(m => m.MissionSkills.Any(s => s.Skill.SkillName == skillname) && m.Country.Name == countryname);
                    }
                }
                filterMissions = filterMissions.Concat(missionsList);

            }

            else if (country.Length != 0)
            {
                foreach (string countryname in country)
                {
                    filterMissions = filterMissions.Concat(missions.Where(u => u.Country.Name == countryname));
                }
            }

            else if (city.Length != 0)
            {
                foreach (string cityname in city)
                {
                    filterMissions = filterMissions.Concat(missions.Where(u => u.City.Name == cityname));
                }
            }

            else if (theme.Length != 0)
            {

                foreach (string themename in theme)
                {
                    filterMissions = filterMissions.Concat(missions.Where(u => u.Theme.Title == themename));

                }
            }

            else if (skill.Length != 0)
            {
                foreach (string skillname in skill)
                {
                    filterMissions = filterMissions.Concat(missions.Where(u => u.MissionSkills.Any(s => s.Skill.SkillName == skillname)));
                }
            }

            else
            {
                filterMissions = missions;
            }


            if (country.Length == 0 && city.Length == 0 && theme.Length == 0 && skill.Length == 0)
            {
                if (search != "")
                {
                    filterMissions = missions.Where(missiondata => missiondata.Title.ToLower().Contains(search) || missiondata.ShortDescription.ToLower().Contains(search));
                }
            }
            else
            {
                if (search != "")
                {
                    filterMissions = filterMissions.Where(missiondata => missiondata.Title.ToLower().Contains(search) || missiondata.ShortDescription.ToLower().Contains(search));
                }
            }
            
            

            if (country.Length == 0 && city.Length == 0 && theme.Length == 0 && skill.Length == 0 && search == "")
            {
                if (sort == null)
                {
                    return missions.Distinct();
                }
                else
                {
                    var missiondata = GetBySortF(sort, missions, userid);

                    missions = missiondata;
                    return missions.Distinct();
                }
            }
            else
            {
                if (sort == null)
                {
                    return filterMissions.Distinct();
                }
                else
                {
                    var missiondata = GetBySortF(sort, filterMissions, userid);

                    filterMissions = missiondata;
                    return filterMissions.Distinct();
                }
            }
            

        }

        public List<Mission> GetBySortF(string sort, IEnumerable<Mission> filterMissions,long userid)
        {

            if (sort == "Oldest")
            {
                return filterMissions.OrderBy(m => m.StartDate).ToList();
            }
            else if (sort == "Newest")
            {
                return filterMissions.OrderByDescending(m => m.StartDate).ToList();
            }
            else if (sort == "Mission Type")
            { 
                return filterMissions.OrderBy(m => m.MissionType).ToList();
            }
            else if (sort == "My favourites")
            {
                
                return filterMissions.OrderByDescending(m => m.FavoriteMissions.Where(user => user.UserId == userid).Count()).ToList();
            }
            else if (sort == "Highest available seats")
            {
                return filterMissions.OrderByDescending(m => m.TotalSheet - m.MissionApplications.Count()).ToList();
            }
            else if (sort == "Lowest available seats")
            {
                return filterMissions.OrderBy(m => m.TotalSheet - m.MissionApplications.Count()).ToList();
            }
            else if (sort == "Default")
            {
                return filterMissions.OrderBy(m => m.MissionId).ToList();
            }
            else if (sort == "Deadline")
            {
                return filterMissions.OrderBy(m => m.Deadline).ToList();
            }
            else
            {
                return filterMissions.OrderBy(m => m.StartDate).ToList();
            }
        }

    }
    }
