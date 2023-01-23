using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using Zadanie_Rekrutacyjne_1.Data.Repository;
using Zadanie_Rekrutacyjne_1.Models;
using System.Xml;
using System.Data;
using ArrayToPdf;
using ClosedXML.Excel;
using Zadanie_Rekrutacyjne_1.Utilities;

namespace Zadanie_Rekrutacyjne_1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<User> users = unitOfWork.UserRepository.GetAll();
            foreach (var user in users)
            {
                user.Gender = unitOfWork.GenderRepository.GetT(x => x.GenderId == user.GenderForeignKey);
                user.AdditionalAttributes = unitOfWork.AttributeRepository.GetAll(x => x.UserId == user.Id).ToList();
            }
            return View(users);
        }
        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            var gendersList = unitOfWork.GenderRepository.GetAll().Select(x => new SelectListItem()
            {
                Text = x.GenderName,
                Value = x.GenderName,
            });
            ViewData["GendersList"] = gendersList;
            User user = new User();
            if (id == null || id == 0)
            {
                user.AdditionalAttributes = new List<AdditionalAttribute>();
                user.AdditionalAttributes.Add(null);
                return View(user);
            }
            else
            {
                user = unitOfWork.UserRepository.GetT(x => x.Id == id);
                user.AdditionalAttributes = unitOfWork.AttributeRepository.GetAll(x => x.UserId == user.Id).ToList();
                user.AdditionalAttributes.Add(null);
                if (user == null)
                    return NotFound();
                else
                    return View(user);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(User user)
        {
            if (ModelState.IsValid)
            {
                user.Gender = unitOfWork.GenderRepository.GetT(x => x.GenderName == user.Gender.GenderName);
                if (user.Id == 0)
                {
                    unitOfWork.UserRepository.Add(user);
                    TempData["success"] = "Dodano użytkownika";
                }
                else
                {
                    unitOfWork.UserRepository.Update(user);
                    TempData["success"] = "Zaktualizowano użytkownika";
                }
                unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

        public FileResult ExportToPdf()
        {
            List<ReportElement> report = GenerateReportModel();
            DateTime now = DateTime.Now;
            string date = now.ToString("MM / dd / yy H: mm:ss");

            byte[] pdf = report.ToPdf();
            return File(pdf, "application/pdf", date + ".pdf");
        }
        public FileResult ExportToExcel()
        {
            List<ReportElement> report = GenerateReportModel();
            DataTable dtReport = GenerateReportDataTable(report);

            DateTime now = DateTime.Now;
            string date = now.ToString("MM / dd / yy H: mm:ss");

            using (XLWorkbook woekBook = new XLWorkbook())
            {
                woekBook.Worksheets.Add(dtReport);
                using (MemoryStream stream = new MemoryStream())
                {
                    woekBook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", date + ".xlsx");
                }
            }
        }
        public FileResult ExportToXML()
        {
            List<ReportElement> report = GenerateReportModel();

            DateTime now = DateTime.Now;
            string date = now.ToString("MM / dd / yy H: mm:ss");

            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Raport");
            xml.AppendChild(root);
            foreach (var item in report)
            {
                XmlElement child = xml.CreateElement("Użytkownik");
                child.SetAttribute("ZwrotGrzecznościowy", item.Status.ToString());
                child.SetAttribute("Imię", item.Name);
                child.SetAttribute("Nazwisko", item.LastName);
                child.SetAttribute("DataNarodzin", item.DateOfBirth.ToString());
                child.SetAttribute("Wiek", item.Age);
                child.SetAttribute("Płeć", item.Gender);
                root.AppendChild(child);
            }
            return File(Encoding.UTF8.GetBytes(xml.OuterXml), "application/xml", date + ".xml");
        }
        public FileResult ExportToTxt(string GridHtml)
        {
            List<ReportElement> report = GenerateReportModel();
            DataTable dtReport = GenerateReportDataTable(report);

            DateTime now = DateTime.Now;
            string date = now.ToString("MM / dd / yy H: mm:ss");

            var delimeter = ",";
            var lineEndDelimeter = ";";
            StringBuilder sb = new StringBuilder();
            string Columns = string.Empty;

            foreach (DataColumn column in dtReport.Columns)
            {
                Columns += column.ColumnName + delimeter;
            }
            sb.Append(Columns.Remove(Columns.Length - 1, 1) + lineEndDelimeter);
            foreach (DataRow datarow in dtReport.Rows)
            {
                string row = string.Empty;
                foreach (object items in datarow.ItemArray)
                {
                    row += items.ToString() + delimeter;
                }

                sb.Append(row.Remove(row.Length - 1, 1) + lineEndDelimeter);
            }

            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/plain", date + ".txt");
        }
        private List<ReportElement> GenerateReportModel()
        {
            List<ReportElement> report = new List<ReportElement>();
            var users = unitOfWork.UserRepository.GetAll();
            foreach (var user in users)
            {
                user.Gender = unitOfWork.GenderRepository.GetT(x => x.GenderId == user.GenderForeignKey);
            }
            foreach (var item in users)
            {
                report.Add(new ReportElement()
                {
                    Status = item.Gender.GenderName == "Mężczyzna" ? MrMsStatus.Pan : MrMsStatus.Pani,  
                    Name = item.Name,
                    LastName = item.LastName,
                    DateOfBirth = item.DateOfBirth,
                    Age = ((DateTime.Now.Year) - (item.DateOfBirth.Year)).ToString(),
                    Gender = item.Gender.GenderName
                });
            }
            return report;
        }
        private DataTable GenerateReportDataTable(List<ReportElement> report)
        {
            DataTable dtReport = new DataTable("Report");
            dtReport.Columns.AddRange(new DataColumn[6] { new DataColumn("Zwrot grzecznościowy"),
                new DataColumn("Imię"),
                new DataColumn("Nazwisko"),
                new DataColumn("DataNarodzin"),
                new DataColumn("Wiek"),
                new DataColumn("Płeć") });

            foreach (var item in report)
            {
                dtReport.Rows.Add(item.Status, item.Name, item.LastName, item.DateOfBirth, item.Age, item.Gender);
            }
            return dtReport;
        }
    }
}
