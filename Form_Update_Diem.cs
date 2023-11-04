﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace QLHS
{
    public partial class Form_Update_Diem : Form
    {
        public SqlConnection conn = new SqlConnection();
        function ham = new function();

        public Form_Update_Diem()
        {
            InitializeComponent();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Form_Home fh = new Form_Home();
            fh.ShowDialog();
        }

        private void print_Click(object sender, EventArgs e)
        {
            Form_Print fp = new Form_Print();
            fp.ShowDialog();
        }

        private void update_thongtin_Click(object sender, EventArgs e)
        {
            Form_Update_ThongTin tt = new Form_Update_ThongTin();
            tt.ShowDialog();
        }






        private void Form_Update_Diem_Load(object sender, EventArgs e)
        {
            ham.connect(conn);
            ham.HienThiDLDG(dataGridView1, "Select d.MaDiem, d.MaHocSinh,hs.HoTen, mh.TenMonHoc, hk.TenHocKy,d.DiemMieng ,d.Diem15Phut,d.Diem1Tiet,d.DiemThi from Diem d,HocSinh hs,HocKy hk,MonHoc mh where d.MaHocSinh = hs.MaHocSinh and d.MaHocKy = hk.MaHocKy and mh.MaMonHoc = d.MaMon;\r\n", conn);
            ham.HienThiDLComb(cb_hoc_ky, "SELECT MAHOCKY, TENHOCKY FROM HOCKY", conn, "TENHOCKY", "MAHOCKY");
            ham.HienThiDLComb(cb_mahs, "SELECT MaHocSinh, HoTen FROM HocSinh", conn, "HoTen", "MaHocSinh");
            ham.HienThiDLComb(cb_mon, "SELECT MaMonHoc, TenMonHoc FROM MonHoc", conn, "TenMonHoc", "MaMonHoc");

        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            string ma_lon_nhat = "select Max (SUBSTRING(MaDiem,3,3)) from DIEM";
            SqlCommand comd = new SqlCommand(ma_lon_nhat, conn);
            SqlDataReader reader = comd.ExecuteReader();

            if (reader.Read())
            {
                int max = Convert.ToInt16(reader.GetValue(0).ToString()) + 1;
                if (max < 10)
                {
                    txt_ma_diem.Text = "MD00" + max;
                }
                else
                {
                    txt_ma_diem.Text = "MD0" + max;
                }
                txt_ma_diem.Enabled = false;
            }
            reader.Close();

            string maDiem = txt_ma_diem.Text;
            string maHocSinh = cb_mahs.SelectedValue.ToString();
            string maMon = cb_mon.SelectedValue.ToString();
            string maHocKy = cb_hoc_ky.SelectedValue.ToString();
            string diemmieng = txt_mieng.Text;
            string diem15 = txt_15p.Text;
            string diem1t = txt_1t.Text;
            string diemthi = txt_thi.Text;

            try
            {
                float diemMiengValue = float.Parse(diemmieng);
                float diem15Value = float.Parse(diem15);
                float diem1tValue = float.Parse(diem1t);
                float diemThiValue = float.Parse(diemthi);

                if (diemMiengValue < 0 || diemMiengValue > 10 ||
                    diem15Value < 0 || diem15Value > 10 ||
                    diem1tValue < 0 || diem1tValue > 10 ||
                    diemThiValue < 0 || diemThiValue > 10)
                {
                    // Xử lý khi giá trị điểm không nằm trong khoảng từ 0 đến 10
                    string diemQuery = $"SELECT MaHocSinh FROM Diem WHERE MaHocSinh = '{maHocSinh}' AND MaMon = '{maMon}' AND MaHocKy = '{maHocKy}'";
                    SqlCommand diemCmd = new SqlCommand(diemQuery, conn);
                    SqlDataReader diemReader = diemCmd.ExecuteReader();

                    if (diemReader.Read())
                    {
                        diemReader.Close(); // Đóng DataReader trước khi thực hiện truy vấn UPDATE
                        string updateQuery = $"UPDATE Diem SET DiemMieng =  (DiemMieng + {diemmieng})/2, Diem15Phut =  (Diem15Phut + {diem15})/2, Diem1Tiet =   (Diem1Tiet + {diem1t})/2, DiemThi =  (DiemThi + {diemthi})/2 WHERE MaHocSinh = '" + maHocSinh + "' AND MaMon = '" + maMon + "' AND MaHocKy = '" + maHocKy + "'";
                        ham.capnhat(updateQuery, conn);
                        ham.HienThiDLDG(dataGridView1, "SELECT * FROM Diem", conn);
                        clearALL();
                    }
                    else
                    {
                        diemReader.Close(); // Đóng DataReader trước khi thực hiện truy vấn INSERT

                        string insertQuery = $"INSERT INTO Diem (MaDiem, MaHocSinh, MaMon, MaHocKy, DiemMieng, Diem15Phut, Diem1Tiet, DiemThi) VALUES ('{maDiem}', '{maHocSinh}', '{maMon}', '{maHocKy}', '{diemmieng}', '{diem15}', '{diem1t}', '{diemthi}')";
                        ham.capnhat(insertQuery, conn);
                        ham.HienThiDLDG(dataGridView1, "SELECT * FROM Diem", conn);
                        clearALL();
                    }
                }
            }
            catch (FormatException)
            {
                // Xử lý khi giá trị điểm không phải là số
                MessageBox.Show("Nhập điểm sai, Vui lòng nhập lại");
            }




        }


        private void btn_sua_Click(object sender, EventArgs e)
        {
            string maDiem = txt_ma_diem.Text;
            string maHocSinh = cb_mahs.SelectedValue.ToString();
            string maMon = cb_mon.SelectedValue.ToString();
            string maHocKy = cb_hoc_ky.SelectedValue.ToString();
            string diemmieng = txt_mieng.Text;
            string diem15 = txt_15p.Text;
            string diem1t = txt_1t.Text;
            string diemthi = txt_thi.Text;
            try
            {
                float diemMiengValue = float.Parse(diemmieng);
                float diem15Value = float.Parse(diem15);
                float diem1tValue = float.Parse(diem1t);
                float diemThiValue = float.Parse(diemthi);

                if (diemMiengValue < 0 || diemMiengValue > 10 ||
                    diem15Value < 0 || diem15Value > 10 ||
                    diem1tValue < 0 || diem1tValue > 10 ||
                    diemThiValue < 0 || diemThiValue > 10)
                {
                    // Xử lý khi giá trị điểm không nằm trong khoảng từ 0 đến 10
                    string query = "UPDATE Diem SET MaHocSinh = '" + maHocSinh + "',MaMon = '" + maMon + "',MaHocKy = '" + maHocKy + "',DiemMieng = '" + diemmieng + "',Diem15Phut = '" + diem15 + "',Diem1Tiet = '" + diem1t + "',DiemThi = '" + diemthi + "' Where MADIEM='" + maDiem + "';";
                    ham.capnhat(query, conn);
                    ham.HienThiDLDG(dataGridView1, "Select * from Diem", conn);
                    clearALL();
                }
            }
            catch (FormatException)
            {
                // Xử lý khi giá trị điểm không phải là số
                MessageBox.Show("Nhập điểm sai, Vui lòng nhập lại");
            }


        }
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            string maDiem = txt_ma_diem.Text;
            string query = "DELETE From Diem Where MaDiem = '" + maDiem + "'";
            ham.capnhat(query, conn);
            ham.HienThiDLDG(dataGridView1, "Select * from Diem", conn);
            clearALL();
        }
        public void clearALL()
        {
            txt_ma_diem.Text = "";
            cb_mahs.SelectedValue = "Chọn học sinh";
            cb_mon.SelectedValue = "Chọn môn học";
            cb_hoc_ky.SelectedValue = "Chọn học kì";
            txt_thi.Text = "";
            txt_15p.Text = "";
            txt_1t.Text = "";
            txt_mieng.Text = "";
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ma_diem.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); ;
            cb_mahs.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); ;
            cb_mon.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(); ;
            cb_hoc_ky.SelectedValue = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(); ;
            txt_mieng.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(); ;
            txt_15p.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString(); ;
            txt_1t.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString(); ;
            txt_thi.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString(); ;
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            clearALL();
        }

        private void cb_mon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

