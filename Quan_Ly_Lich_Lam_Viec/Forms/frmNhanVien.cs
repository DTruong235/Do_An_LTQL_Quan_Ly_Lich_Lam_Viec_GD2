using Quan_Ly_Lich_Lam_Viec.Data;


namespace Quan_Ly_Lich_Lam_Viec.Forms
{
    public partial class frmNhanVien : Form
    {
        public frmNhanVien()
        {
            InitializeComponent();
        }

        Quan_Li_Lich_Lam_DbContext context = new Quan_Li_Lich_Lam_DbContext();
        bool xuLyThem = false; // Cờ kiểm tra đang thêm hay sửa

        private void BatTatChucNang(bool giaTri)
        {
            btnLuu.Enabled = giaTri;
            btnHuyBo.Enabled = giaTri;

            // Vùng nhập liệu
            txtHoVaTen.Enabled = giaTri;
            txtEmail.Enabled = giaTri;
            txtSDT.Enabled = giaTri;
            dtpNgaySinh.Enabled = giaTri;
            cboMaPhong.Enabled = giaTri;
            cboMaChucVu.Enabled = giaTri;
            txtDiaChi.Enabled = giaTri;

            // Các nút thao tác
            btnThem.Enabled = !giaTri;
            btnSua.Enabled = !giaTri;
            btnXoa.Enabled = !giaTri;
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false); // Mặc định khóa nhập liệu [cite: 74]
            LoadData();
        }

        private void LoadData()
        {
            // Load dữ liệu cho ComboBox (Phòng ban & Chức vụ)
            cboMaPhong.DataSource = context.Phong_Ban.ToList();
            cboMaPhong.DisplayMember = "TenPhong"; // Hiển thị tên
            cboMaPhong.ValueMember = "MaPhong";    // Lưu mã

            cboMaChucVu.DataSource = context.Chuc_Vu.ToList();
            cboMaChucVu.DisplayMember = "TenChucVu";
            cboMaChucVu.ValueMember = "MaChucVu";

            // Load danh sách nhân viên bằng BindingSource
            var listNV = context.Nhan_Vien.ToList();
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = listNV;

            // Binding dữ liệu vào TextBox (Kỹ thuật DataBindings)
            txtHoVaTen.DataBindings.Clear();
            txtHoVaTen.DataBindings.Add("Text", bindingSource, "HoTen", false, DataSourceUpdateMode.Never);

            txtEmail.DataBindings.Clear();
            txtEmail.DataBindings.Add("Text", bindingSource, "Email", false, DataSourceUpdateMode.Never);

            // Binding cho ComboBox (Tự động nhảy đúng phòng ban khi chọn nhân viên)
            cboMaPhong.DataBindings.Clear();
            cboMaPhong.DataBindings.Add("SelectedValue", bindingSource, "MaPhong", false, DataSourceUpdateMode.Never);

            cboMaChucVu.DataBindings.Clear();
            cboMaChucVu.DataBindings.Add("SelectedValue", bindingSource, "MaChucVu", false, DataSourceUpdateMode.Never);

            // Binding cho Địa chỉ
            txtDiaChi.DataBindings.Clear();
            txtDiaChi.DataBindings.Add("Text", bindingSource, "DiaChi", false, DataSourceUpdateMode.Never);

            dataGridView.DataSource = bindingSource;

            // Đưa lên lưới
            dataGridView.DataSource = bindingSource;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            xuLyThem = true;
            BatTatChucNang(true); // Mở khóa nhập liệu

            // Xóa trắng các ô để nhập mới
            txtHoVaTen.Clear();
            txtEmail.Clear();
            txtSDT.Clear();
            cboMaPhong.SelectedIndex = -1;
            cboMaChucVu.SelectedIndex = -1;
            txtDiaChi.Clear();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;
            xuLyThem = false;
            BatTatChucNang(true); // Mở khóa để sửa
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;

            // Hỏi xác nhận xóa [cite: 134]
            if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dataGridView.CurrentRow.Cells["MaNhanVien"].Value);
                Nhan_Vien nv = context.Nhan_Vien.Find(id);
                if (nv != null)
                {
                    context.Nhan_Vien.Remove(nv);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Validate dữ liệu
            if (string.IsNullOrWhiteSpace(txtHoVaTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!");
                return;
            }

            try
            {
                if (xuLyThem) // Logic Thêm Mới
                {
                    Nhan_Vien nv = new Nhan_Vien();
                    nv.HoTen = txtHoVaTen.Text;
                    nv.Email = txtEmail.Text;
                    nv.SoDienThoai = txtSDT.Text;
                    nv.NgaySinh = dtpNgaySinh.Value;
                    nv.MaPhong = (int)cboMaPhong.SelectedValue; // Lấy ID từ ComboBox
                    nv.MaChucVu = (int)cboMaChucVu.SelectedValue;
                    nv.DiaChi = txtDiaChi.Text;

                    context.Nhan_Vien.Add(nv);
                    context.SaveChanges(); // Lưu xuống DB
                }
                else // Logic Cập Nhật (Sửa)
                {
                    // Lấy ID từ dòng đang chọn
                    int id = Convert.ToInt32(dataGridView.CurrentRow.Cells["MaNhanVien"].Value);
                    Nhan_Vien nv = context.Nhan_Vien.Find(id);

                    if (nv != null)
                    {
                        nv.HoTen = txtHoVaTen.Text;
                        nv.Email = txtEmail.Text;
                        nv.SoDienThoai = txtSDT.Text;
                        nv.NgaySinh = dtpNgaySinh.Value;
                        nv.MaPhong = (int)cboMaPhong.SelectedValue;
                        nv.MaChucVu = (int)cboMaChucVu.SelectedValue;
                        nv.DiaChi = txtDiaChi.Text;

                        context.Nhan_Vien.Update(nv);
                        context.SaveChanges();
                    }
                }

                LoadData(); // Load lại lưới [cite: 128]
                BatTatChucNang(false);
                MessageBox.Show("Lưu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            LoadData(); // Reset lại dữ liệu cũ
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {

        }

        private void btnNhap_Click(object sender, EventArgs e)
        {

        }

        private void btnXuat_Click(object sender, EventArgs e)
        {

        }
    }
}
