﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using EXCELL = Microsoft.Office.Interop.Excel;
using PhamTrongTruong_5951071113.Models;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System.Web.Script.Serialization;

namespace PhamTrongTruong_5951071113.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            return View();

        }
        
        public ActionResult baihoc()
        {
            return View(new TracNghiemDB().Bai_Hoc.Where(x=>x.Xoa==true).ToList());
        }
        public ActionResult QuanLyTaiKhoan()
        {
            return View(new TracNghiemDB().TaiKhoans.Where(x=>x.Quyen==false && x.TrangThai==true).ToList());
        }
        public ActionResult Dscauhoi(long id)
        {
            Session["mabai"] = id;
            return View(new TracNghiemDB().KhoCauHois.Where(x=>x.Ma_Bai==id && x.Xoa==true));
        }
        public ActionResult Taocauhoi()
        {
            List<KhoCauHoi> cauHois = (List<KhoCauHoi>)Session["CH"];
            try
            {
                if (cauHois.Count == 0)
                {
                    cauHois = new List<KhoCauHoi>();
                }
            }
            catch { cauHois = new List<KhoCauHoi>(); }
            ViewBag.cauhoi = cauHois;
            return View();
        }

        public ActionResult LuuCH()
        {
            List<KhoCauHoi> cauHois = (List<KhoCauHoi>)Session["CH"];
            long id = (long)Session["mabai"];
            foreach (var item in cauHois)
            {
                try{
                    KhoCauHoi khoCauHoi = new KhoCauHoi();
                    khoCauHoi.NoiDung = item.NoiDung;
                    khoCauHoi.HinhAnh = item.HinhAnh;
                    khoCauHoi.MucDọ = item.MucDọ;
                    khoCauHoi.Xoa = true;
                    khoCauHoi.Ma_Bai =(int)id;
                  //  khoCauHoi.D_An = item.D_An;
                    TracNghiemDB db = new TracNghiemDB();
                    db.KhoCauHois.Add(khoCauHoi);
                    db.SaveChanges();
                    khoCauHoi = new TracNghiemDB().KhoCauHois.ToList().Last();
                    foreach (var item1 in item.D_An)
                    {
                        item1.Ma_CH = khoCauHoi.Ma_CH;
                       
                        db.D_An.Add(item1);
                        db.SaveChanges();

                    }
                }
                 catch ( Exception e) {
                    Console.WriteLine(e.Message);
                   }
            }
                Session["CH"]=null;
            ViewBag.cauhoi = cauHois;
        
            return RedirectToAction("Dscauhoi/" + id, "Admin");
       
        }
        public ActionResult LoadCauHoi()
        {
            List<KhoCauHoi> cauHois = (List<KhoCauHoi>)Session["CH"];
            ViewBag.cauhoi = cauHois;
            return View();
        }
        public void LuuCauHoi(string listCH)
        {
            var lisch = new JavaScriptSerializer().Deserialize<List<KhoCauHoi>>(listCH);
            foreach (KhoCauHoi item in lisch)
            {
                if(item.NoiDung.Contains("$c$4"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDọ = 4;
                }
                else if (item.NoiDung.Contains("$c$3"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDọ = 3;
                }
                else if(item.NoiDung.Contains("$c$2"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDọ = 2;
                }
                else  
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDọ = 1;
                }
                foreach (var item1 in item.D_An)
                {
                    if (item1.NoiDung.Contains("$*$"))
                    {
                        item1.NoiDung = item1.NoiDung.Substring(3);
                        item1.TrangThai = true;
                    }
                    else
                    {
                        item1.TrangThai = false;
                    }

                }
            }
            if (lisch[lisch.Count - 1].NoiDung == null)
            {
                lisch.RemoveAt(lisch.Count - 1);
            }
            Session["CH"] = lisch;

        }
      public JsonResult Saveanh(HttpPostedFileBase file)
        {
            string path = "";
          string strExtexsion = Path.GetFileName(file.FileName).Trim();
             path = Server.MapPath("~/Content/" + file.FileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                file.SaveAs(path);
            }
            catch { }
            path = "/Content/" + file.FileName;
            return Json(new { path },JsonRequestBehavior.AllowGet);
        }
        public JsonResult XuLyFile(HttpPostedFileBase file)
        {
            List<KhoCauHoi> cauHois = new List<KhoCauHoi>();
            string strExtexsion = Path.GetFileName(file.FileName).Trim();

            if (strExtexsion.Contains(".xls"))
            {
                try
                {
                    string path = Server.MapPath("~/Content/" + file.FileName);
                    try
                    {
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        file.SaveAs(path);
                    }
                    catch { }


                    EXCELL.Application application = new EXCELL.Application();
                    EXCELL.Workbook workbook = application.Workbooks.Open(path);
                    EXCELL.Worksheet worksheet = workbook.ActiveSheet;


                    EXCELL.Range range = worksheet.UsedRange;

                    cauHois = new List<KhoCauHoi>();

                    for (int i = 2; i <= range.Rows.Count; i++)
                    {
                        try
                        {

                            KhoCauHoi cauHoi = new KhoCauHoi();

                            cauHoi.NoiDung = ((EXCELL.Range)range.Cells[i, 1]).Text;

                            cauHoi.HinhAnh = ((EXCELL.Range)range.Cells[i, 8]).Text;


                            if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("1"))
                            {
                                cauHoi.MucDọ =1;
                            }
                            else if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("2"))
                            {
                                cauHoi.MucDọ =2;
                            }
                            else if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("3"))
                            {
                                cauHoi.MucDọ = 3;
                            }
                            else
                            {
                                cauHoi.MucDọ= 4;
                            }




                            cauHoi.D_An = new List<D_An>();
                            D_An dapAn = new D_An();
                            dapAn.NoiDung = "A) " + ((EXCELL.Range)range.Cells[i, 2]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 9]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("A"))
                            {
                                dapAn.TrangThai = true;
                            }


                            else { dapAn.TrangThai = false; }
                            cauHoi.D_An.Add(dapAn);
                            dapAn = new D_An();
                            dapAn.NoiDung = "B) " + ((EXCELL.Range)range.Cells[i, 3]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 10]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("B"))
                            {
                                dapAn.TrangThai = true;
                            }
                            else { dapAn.TrangThai = false; }
                            cauHoi.D_An.Add(dapAn);
                            dapAn = new D_An();
                            dapAn.NoiDung = "C) " + ((EXCELL.Range)range.Cells[i, 4]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 11]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("C"))
                            {
                                dapAn.TrangThai = true;
                            }
                            else { dapAn.TrangThai = false; }
                            cauHoi.D_An.Add(dapAn);

                            dapAn = new D_An();
                            dapAn.NoiDung = "D) " + ((EXCELL.Range)range.Cells[i, 5]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 12]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("D"))
                            {
                                dapAn.TrangThai = true;
                            }
                            else { dapAn.TrangThai = false; }
                            cauHoi.D_An.Add(dapAn);

                            cauHois.Add(cauHoi);

                        }
                        catch
                        {

                        }
                    }


                    application.Workbooks.Close();
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch { }

                    foreach (var item in cauHois)
                    {


                        try
                        {
                            if (item.HinhAnh.Length > 0)
                            {
                                item.HinhAnh = copyanh(item.HinhAnh.Trim());

                            }
                            foreach (var item1 in item.D_An)
                            {

                                if (item1.HinhAnh.Length > 0)
                                {
                                    item1.HinhAnh = copyanh(item1.HinhAnh.Trim());

                                }
                            }
                        }
                        catch (Exception e)
                        {
                           // MessageBox.Show(e.Message);
                        }

                    }
                    Session["CH"] = cauHois;
                    return Json(new
                    {
                        status = true
                    });


                }
                catch
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            else
            {
                DateTime aDateTime = DateTime.Now;
                object path = Server.MapPath("~/Content/" + file.FileName);
                if (System.IO.File.Exists(path.ToString()))
                {
                    System.IO.File.Delete(path.ToString());
                }

                file.SaveAs(path.ToString());

                List<String> anh = new List<string>();
                string totalText = "";
                Document document = new Document(path.ToString());
                //Get Each Section of Document  
                foreach (Section section in document.Sections)
                {
                    //Get Each Paragraph of Section  
                    foreach (Paragraph paragraph in section.Paragraphs)
                    {
                        //Get Each Document Object of Paragraph Items  
                        foreach (DocumentObject docObject in paragraph.ChildObjects)
                        {
                            //If Type of Document Object is Picture, Extract.  
                            if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                            {
                                int sas = new Random().Next(1000000);
                                    //Convert.ToInt32(aDateTime.Year * 12 * 30 * 24 * 60 * 60 + aDateTime.Month * 30 * 24 * 60 * 60 + aDateTime.Day * 24 * 60 * 60 + aDateTime.Hour * 60 * 60 + aDateTime.Minute * 60 + aDateTime.Second);
                                DocPicture pic = docObject as DocPicture;
                                String imgName = Server.MapPath("~/Content/Img/Anh" + sas + String.Format(".png"));
                                anh.Add("/Content/Img/Anh" + sas + String.Format(".png"));
                                //Save Image  
                                pic.Image.Save(imgName, System.Drawing.Imaging.ImageFormat.Png);

                            }

                        }
                        totalText += paragraph.Text.ToString();

                    }

                }
                int slanh = 0;
                List<D_An> dapan2 = new List<D_An>();
                cauHois = new List<KhoCauHoi>();
                for (int i = 0; i < totalText.Length; i++)
                {
                    if (totalText[i] == '$' && totalText[i + 1] == 'c' && totalText[i + 2] == '$')
                    {
                        int slcau = 0;
                        KhoCauHoi ch = new KhoCauHoi();
                        int sldapa = 0;
                        int slda = 0;
                        List<D_An> dapan = new List<D_An>();

                        ch.D_An = new List<D_An>();
                        for (int j = i; j < totalText.Length; j++)
                        {

                            if ((totalText[j] == '$' && totalText[j + 1] == '*' && totalText[j + 2] == '$') || (totalText[j] == '$' && totalText[j + 1] == '$'))
                            {
                                slcau++;
                                D_An da = new D_An();
                                if (slcau == 1)
                                {
                                    ch.NoiDung = totalText.Substring(i + 3, j - i - 3);
                                    if (ch.NoiDung[0] == '1')
                                    {
                                        ch.MucDọ = 1;
                                    }
                                    else if (ch.NoiDung[0] == '2')
                                    {
                                        ch.MucDọ = 2;
                                    }
                                    else if (ch.NoiDung[0] == '3')
                                    {
                                        ch.MucDọ = 3;
                                    }
                                    else if (ch.NoiDung[0] == '4')
                                    {
                                        ch.MucDọ = 4;
                                    }
                                    else ch.MucDọ =5;
                                    ch.NoiDung = ch.NoiDung.Substring(1, ch.NoiDung.Length - 1);
                                    ch.HinhAnh = "";
                                    for (int z = 0; z < ch.NoiDung.Length - 2; z++)
                                    {
                                        if (ch.NoiDung[z] == '#' && ch.NoiDung[z + 1] == 'h' && ch.NoiDung[z + 2] == '#')
                                        {

                                            ch.HinhAnh = anh[slanh];
                                            slanh++;
                                            ch.NoiDung = ch.NoiDung.Substring(0, z);
                                            break;
                                        }

                                    }

                                }


                                if (ch.MucDọ ==5) break;
                                for (int k = j + 2; k < totalText.Length; k++)
                                {


                                    if (totalText[j] == '$' && totalText[j + 1] == '*' && totalText[j + 2] == '$')
                                    {

                                        if (totalText[k] == '$' && totalText[k + 1] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 3, k - j - 3);
                                            da.TrangThai = true;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            j = k - 1;
                                            ch.D_An.Add(da);


                                        }
                                        else if (totalText[k] == '$' && totalText[k + 1] == 'c' && totalText[k + 2] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 3, k - 3 - j);
                                            da.TrangThai = true;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            j = k - 1;
                                            ch.D_An.Add(da);
                                            break;
                                        }
                                        else if (k == totalText.Length - 1)
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 3, totalText.Length - j - 3);
                                            da.TrangThai = true;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            j = totalText.Length - 1;
                                            ch.D_An.Add(da);
                                            break;
                                        }
                                    }

                                    else if (totalText[j] == '$' && totalText[j + 1] == '$')
                                    {

                                        if (totalText[k] == '$' && totalText[k + 1] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, k - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            j = k - 1;
                                            ch.D_An.Add(da);


                                        }
                                        else if (totalText[k] == '$' && totalText[k + 1] == '*' && totalText[k + 2] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, k - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            j = k - 1;
                                            ch.D_An.Add(da);

                                        }
                                        else if (totalText[k] == '$' && totalText[k + 1] == 'c' && totalText[k + 2] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, k - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            j = k - 1;
                                            ch.D_An.Add(da);
                                            break;
                                        }
                                        else if (k == totalText.Length - 1)
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, totalText.Length - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            ch.D_An.Add(da);
                                            j = totalText.Length - 1;
                                            break;
                                        }
                                    }

                                }



                            }



                            if (sldapa != 0)
                            {
                                cauHois.Add(ch);
                                break;
                            }


                        }
                    }


                }
                Session["CH"] = cauHois;
            }

            return Json(new
            {
                status = true
            });

        }

        public void save_file_from_url(string file_name, string url)
        {
            byte[] content;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();


            Stream stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                content = br.ReadBytes(500000);
                br.Close();
            }
            response.Close();

            FileStream fs = new FileStream(file_name, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(content);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
        }

        public string copyanh(string path)
        {

            DateTime aDateTime = DateTime.Now;
            int sas = Convert.ToInt32(aDateTime.Year * 12 * 30 * 24 * 60 * 60 + aDateTime.Month * 30 * 24 * 60 * 60 + aDateTime.Day * 24 * 60 * 60 + aDateTime.Hour * 60 * 60 + aDateTime.Minute * 60 + aDateTime.Second);
            string Filename = "Anh" + sas + ".jpg";
            string saveLocation = "~/Content/Img/" + Filename;
            string file_name = Server.MapPath(saveLocation);
            if (path.Contains("https"))
            {
                save_file_from_url(file_name, path);
            }
            else
            {
                // path = Server.MapPath(path);
                string path1 = System.IO.Path.GetFullPath(path);
                Image png = Image.FromFile(path1);
                png.Save(file_name, System.Drawing.Imaging.ImageFormat.Jpeg);
                png.Dispose();

            }

            return "/Content/Img/" + Filename;
        }
    }
}