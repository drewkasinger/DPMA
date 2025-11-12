using C971.Models;
using Degree_Plan_Mobile_App.Models;
using Degree_Plan_Mobile_App.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static C971.Services.DegreePlanService;

namespace C971.Services
{
    public static class DegreePlanService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if(db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "DegreePlanV2.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<LoginModel>();
            await db.CreateTableAsync<StudentModel>();
            await db.CreateTableAsync<TermsModel>();
            await db.CreateTableAsync<CoursesModel>();
            await db.CreateTableAsync<DetailedCourseModel>();
            await db.CreateTableAsync<OAAssessmentModel>();
            await db.CreateTableAsync<PAAssessmentModel>();
        }

        internal class Login
        {
            public static async Task<IEnumerable<StudentModel>> SearchForAccount()
            {
                await Init();

                var account = await db.Table<StudentModel>().ToListAsync();
                return account;
            }

            public static async Task CreateAccount(string username, string password, string name)
            {
                await Init();
                LoginModel account = new StudentModel
                {
                    Username = username,
                    Password = password,
                    StudentName = name,
                };

                await db.InsertAsync(account);

                var accountId = account.LoginId;
            }
        }

        internal class Terms
        {
            public static async Task AddTerm(int accountid, string termname, string termstart, string termend)
            {
                await Init();
                var termModel = new TermsModel
                {
                    accountId = accountid,
                    termName = termname,
                    termStart = termstart,
                    termEnd = termend
                };

                await db.InsertAsync(termModel);

                var termid = termModel.termId;
            }

            public static async Task UpdateTerm(int termid, string termnane, string termstart, string termend)
            {
                await Init();
                var termQuery = await db.Table<TermsModel>()
                    .Where(i => i.termId == termid)
                    .FirstOrDefaultAsync();

                if (termQuery != null)
                {
                    termQuery.termName = termnane;
                    termQuery.termStart = termstart;
                    termQuery.termEnd = termend;

                    await db.UpdateAsync(termQuery);
                }
            }

            public static async Task DeleteTerm(int accountid)
            {
                await Init();

                await db.DeleteAsync<TermsModel>(accountid);

                await DegreePlanService.Courses.DeleteAllCourses(accountid);
            }

            public static async Task<IEnumerable<TermsModel>> GetTerms(int accountid)
            {
                await Init();

                var terms = await db.Table<TermsModel>().Where(i => i.accountId == accountid).ToListAsync();
                return terms;
            }

            public static async Task<IEnumerable<TermsModel>> GetSelectedTerm(int termid)
            {
                await Init();

                var selectedTerm = await db.Table<TermsModel>().Where(i => i.termId == termid).ToListAsync();
                return selectedTerm;
            }
        }

        internal class Courses
        {
            public static async Task AddCourses(int termid, string statuspicker, string coursename, DateTime coursestart, DateTime courseend)
            {
                await Init();
                var courseModel = new CoursesModel
                {
                    termId = termid,
                    statusPicker = statuspicker,
                    courseName = coursename,
                    courseStart = coursestart.ToShortDateString(),
                    courseEnd = courseend.ToShortDateString()
                };

                await db.InsertAsync(courseModel);

                var courseid = courseModel.courseId;
            }

            public static async Task UpdateCourses(int courseid, string statuspicker, string coursename, DateTime coursestart, DateTime courseend)
            {
                await Init();
                var courseQuery = await db.Table<CoursesModel>()
                    .Where(i => i.courseId == courseid)
                    .FirstOrDefaultAsync();

                if (courseQuery != null)
                {
                    courseQuery.statusPicker = statuspicker;
                    courseQuery.courseName = coursename;
                    courseQuery.courseStart = coursestart.ToShortDateString();
                    courseQuery.courseEnd = courseend.ToShortDateString();

                    await db.UpdateAsync(courseQuery);
                }
            }

            public static async Task DeleteCourses(int courseid)
            {
                await Init();

                await db.DeleteAsync<CoursesModel>(courseid);
                await db.DeleteAsync<DetailedCourseModel>(courseid);
                await db.DeleteAsync<OAAssessmentModel>(courseid);
                await db.DeleteAsync<PAAssessmentModel>(courseid);
            }
            public static async Task DeleteAllCourses(int termid)
            {
                await Init();

                await db.DeleteAsync<CoursesModel>(termid);
            }

            public static async Task<IEnumerable<CoursesModel>> GetTermCourses(int termid)
            {
                await Init();

                var courses = await db.Table<CoursesModel>().Where(i => i.termId == termid).ToListAsync();
                return courses;
            }
            public static async Task<IEnumerable<CoursesModel>> GetSelectedCourse(int courseid)
            {
                await Init();

                var courses = await db.Table<CoursesModel>().Where(i => i.courseId == courseid).ToListAsync();
                return courses;
            }
        }

        internal class DetailedCourse()
        {
            public static async Task<IEnumerable<DetailedCourseModel>> GetCourseDetails(int courseid)
            {
                await Init();

                var courses = await db.Table<DetailedCourseModel>().Where(i => i.courseId == courseid).ToListAsync();
                return courses;
            }

            public static async Task AddDetailedCourse(int courseid, string statuspicker, DateTime duedate, DateTime startnotification, 
                DateTime endnotification, string optionalnotes, string instructorname, string instructorphone, string instructoremail)
            {
                await Init();
                var courseModel = new DetailedCourseModel
                {
                    courseId = courseid,
                    statusPicker = statuspicker,
                    dueDate = duedate,
                    startNotification = startnotification,
                    endNotification = endnotification,
                    optionalNotes = optionalnotes,
                    instructorName = instructorname,
                    instructorPhone = instructorphone,
                    instructorEmail = instructoremail
                };

                await db.InsertAsync(courseModel);

                var dcid = courseModel.dcID;
            }

            public static async Task UpdateDetailedCourse(int courseid, string statuspicker, DateTime duedate, DateTime startnotification, DateTime endnotification, string optionalnotes, string instructorname, string instructorphone, string instructoremail)
            {
                await Init();

                var detailedQuery = await db.Table<DetailedCourseModel>()
                    .Where(i => i.courseId == courseid)
                    .FirstOrDefaultAsync();

                if(detailedQuery != null)
                {
                    detailedQuery.statusPicker = statuspicker;
                    detailedQuery.dueDate = duedate;
                    detailedQuery.startNotification = startnotification;
                    detailedQuery.endNotification = endnotification;
                    detailedQuery.optionalNotes = optionalnotes;
                    detailedQuery.instructorName = instructorname;
                    detailedQuery.instructorPhone = instructorphone;
                    detailedQuery.instructorEmail = instructoremail;

                    await db.UpdateAsync(detailedQuery);
                }
            }
        }

        internal class Assessments()
        {
            public static async Task AddOA(int courseid, string oa, string start, string end)
            {
                await Init();
                var oaModel = new OAAssessmentModel
                {
                    courseId = courseid,
                    OA = oa,
                    OAStart = DateTime.Parse(start),
                    OAEnd = DateTime.Parse(end),
                };

                await db.InsertAsync(oaModel);

                var OAID = oaModel.OAID;
            }

            public static async Task AddPA(int courseid, string pa, string start, string end)
            {
                await Init();
                var paModel = new PAAssessmentModel
                {
                    courseId = courseid,
                    PA = pa,
                    PAStart = DateTime.Parse(start),
                    PAEnd = DateTime.Parse(end),
                };

                await db.InsertAsync(paModel);

                var PAID = paModel.PAID;
            }

            public static async Task UpdateOAAssessment(int oaid, string oa, string start, string end)
            {
                await Init();
                var assessmentQuery = await db.Table<OAAssessmentModel>()
                    .Where(i => i.OAID == oaid)
                    .FirstOrDefaultAsync();

                if (assessmentQuery != null)
                {
                    assessmentQuery.OA = oa;
                    assessmentQuery.OAStart = DateTime.Parse(start);
                    assessmentQuery.OAEnd = DateTime.Parse(end);

                    await db.UpdateAsync(assessmentQuery);
                }
            }
            public static async Task UpdatePAAssessment(int paid, string pa, string start, string end)
            {
                await Init();
                var assessmentQuery = await db.Table<PAAssessmentModel>()
                    .Where(i => i.PAID == paid)
                    .FirstOrDefaultAsync();

                if (assessmentQuery != null)
                {
                    assessmentQuery.PA = pa;
                    assessmentQuery.PAStart = DateTime.Parse(start);
                    assessmentQuery.PAEnd = DateTime.Parse(end);

                    await db.UpdateAsync(assessmentQuery);
                }
            }

            public static async Task DeleteOAAssessment(int id)
            {
                await Init();

                await db.DeleteAsync<OAAssessmentModel>(id);
            }

            public static async Task DeletePAAssessment(int id)
            {
                await Init();

                await db.DeleteAsync<PAAssessmentModel>(id);
            }

            public static async Task<IEnumerable<OAAssessmentModel>> GetOAAssessment(int courseid)
            {
                await Init();

                var oaassessment = await db.Table<OAAssessmentModel>().Where(i => i.courseId == courseid).ToListAsync();
                return oaassessment;
            }

            public static async Task<IEnumerable<PAAssessmentModel>> GetPAAssessment(int courseid)
            {
                await Init();

                var paassessment = await db.Table<PAAssessmentModel>().Where(i => i.courseId == courseid).ToListAsync();
                return paassessment;
            }
        }

        public static async Task CreateTestAccount()
        {
            await Init();

            LoginModel login1 = new StudentModel()
            {
                Username = "test",
                Password = "test",
                StudentName = "test name",
            };

            await db.InsertAsync(login1);
        }

        public static async Task LoadSampleData()
        {
            await Init();

            DateTime start = DateTime.Parse(DateTime.Now.AddMonths(1).ToString("MM/01/yyyy"));
            TermsModel term1 = new TermsModel()
            {
                accountId = 1,
                termName = "First Term!",
                termStart = start.ToShortDateString(),
                termEnd = start.AddMonths(6).AddDays(-1).ToShortDateString(),
            };

            await db.InsertAsync(term1);

            CoursesModel course1 = new CoursesModel
            {
                termId = term1.termId,
                statusPicker = "Not Started",
                courseName = "First Course!",
                courseStart = term1.termStart,
                courseEnd = DateTime.Parse(term1.termStart).AddMonths(1).AddDays(-1).ToShortDateString()
            };

            await db.InsertAsync(course1);

            DetailedCourseModel detailedcourse = new DetailedCourseModel
            {
                courseId = course1.courseId,
                statusPicker = course1.statusPicker,
                dueDate = DateTime.Parse(course1.courseEnd),
                startNotification = DateTime.Parse(course1.courseStart).AddDays(-7),
                endNotification = DateTime.Parse(course1.courseEnd).AddDays(-7),
                instructorName = "Anika Patel",
                instructorPhone = "555-123-4567",
                instructorEmail = "anika.patel@strimeuniversity.edu"
            };

            await db.InsertAsync(detailedcourse);

            OAAssessmentModel oaassessment = new OAAssessmentModel
            {
                //courseId = detailedcourse.dcID,
                courseId = course1.courseId,
                OA = "First OA",
                OAStart = DateTime.Parse(course1.courseStart).AddDays(-7),
                OAEnd = DateTime.Parse(course1.courseEnd).AddDays(-7),
            };

            await db.InsertAsync(oaassessment);

            PAAssessmentModel paassessment = new PAAssessmentModel
            {
                //courseId = detailedcourse.dcID,
                courseId = course1.courseId,
                PA = "First PA",
                PAStart = DateTime.Parse(course1.courseStart).AddDays(-7),
                PAEnd = DateTime.Parse(course1.courseEnd).AddDays(-7),
            };

            await db.InsertAsync(paassessment);
        }
        public static async Task DeleteTables()
        {
            await Init();

            await db.DropTableAsync<PAAssessmentModel>();
            await db.DropTableAsync<OAAssessmentModel>();
            await db.DropTableAsync<DetailedCourseModel>();
            await db.DropTableAsync<CoursesModel>();
            await db.DropTableAsync<TermsModel>();
            await db.DropTableAsync<StudentModel>();
            await db.DropTableAsync<LoginModel>();

            db = null;
        }
    }
}
