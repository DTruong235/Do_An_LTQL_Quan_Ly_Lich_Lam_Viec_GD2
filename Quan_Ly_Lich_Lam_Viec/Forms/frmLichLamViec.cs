using Microsoft.EntityFrameworkCore;
using Quan_Ly_Lich_Lam_Viec.Data;
using System.Data;

namespace Quan_Ly_Lich_Lam_Viec.Forms
{
    public partial class frmLichLamViec : Form
    {
        public frmLichLamViec()
        {
            InitializeComponent();
        }

        Quan_Li_Lich_Lam_DbContext context = new Quan_Li_Lich_Lam_DbContext();
        bool xuLyThem = false;

        private void frmLichLamViec_Load(object sender, EventArgs e)
        {
            BatTatChucNang(false); // Khóa các ô nhập khi mới mở
            LoadData(); // Tải dữ liệu lên
        }

        private void BatTatChucNang(bool bat)
        {
            // Nhóm nút lệnh
            btnLuu.Enabled = bat;
            btnHuyBo.Enabled = bat;
            btnThem.Enabled = !bat;
            btnSua.Enabled = !bat;
            btnXoa.Enabled = !bat;

            // Nhóm nhập liệu
            txtTieuDe.Enabled = bat;
            rtbNoiDung.Enabled = bat;
            dtpBatDau.Enabled = bat;
            dtpKetThuc.Enabled = bat;
            cboLoaiCongViec.Enabled = bat;
            cboDiaDiem.Enabled = bat;
        }

        private void LoadData()
        {
            // Load ComboBox Loại Công Việc
            cboLoaiCongViec.DataSource = context.Loai_Cong_Viec.ToList();
            cboLoaiCongViec.DisplayMember = "TenLoai";
            cboLoaiCongViec.ValueMember = "MaLoaiCV";

            // Load ComboBox Địa Điểm
            cboDiaDiem.DataSource = context.Dia_Diem.ToList();
            cboDiaDiem.DisplayMember = "TenDiaDiem";
            cboDiaDiem.ValueMember = "MaDiaDiem";

            // Load Danh sách Lịch (Có Include để lấy thông tin bảng phụ)
            var listLich = context.Lich_Lam_Viec
                                  .Include(l => l.Loai_Cong_Viec)
                                  .Include(l => l.Dia_Diem)
                                  .OrderByDescending(l => l.ThoiGianBatDau) // Lịch mới nhất lên đầu
                                  .ToList();

            // Đổ dữ liệu vào BindingSource
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = listLich;

            // Binding (Liên kết ngược) từ lưới lên các ô nhập
            // Lưu ý: Phải Clear trước khi Add để tránh lỗi
            txtTieuDe.DataBindings.Clear();
            txtTieuDe.DataBindings.Add("Text", bindingSource, "TieuDe", true, DataSourceUpdateMode.Never);

            rtbNoiDung.DataBindings.Clear();
            rtbNoiDung.DataBindings.Add("Text", bindingSource, "NoiDung", true, DataSourceUpdateMode.Never);

            dtpBatDau.DataBindings.Clear();
            dtpBatDau.DataBindings.Add("Value", bindingSource, "ThoiGianBatDau", true, DataSourceUpdateMode.Never);

            dtpKetThuc.DataBindings.Clear();
            dtpKetThuc.DataBindings.Add("Value", bindingSource, "ThoiGianKetThuc", true, DataSourceUpdateMode.Never);

            cboLoaiCongViec.DataBindings.Clear();
            cboLoaiCongViec.DataBindings.Add("SelectedValue", bindingSource, "MaLoaiCV", true, DataSourceUpdateMode.Never);

            cboDiaDiem.DataBindings.Clear();
            cboDiaDiem.DataBindings.Add("SelectedValue", bindingSource, "MaDiaDiem", true, DataSourceUpdateMode.Never); // Chú ý nếu MaDiaDiem null sẽ cần xử lý thêm

            dataGridView.DataSource = bindingSource;

            // Ẩn các cột ID không cần thiết trên lưới
            if (dataGridView.Columns["MaLich"] != null) dataGridView.Columns["MaLich"].Visible = false;
            if (dataGridView.Columns["MaLoaiCV"] != null) dataGridView.Columns["MaLoaiCV"].Visible = false;
            if (dataGridView.Columns["MaDiaDiem"] != null) dataGridView.Columns["MaDiaDiem"].Visible = false;
            if (dataGridView.Columns["LoaiCongViec"] != null) dataGridView.Columns["LoaiCongViec"].Visible = false;
            if (dataGridView.Columns["DiaDiem"] != null) dataGridView.Columns["DiaDiem"].Visible = false;

            // Ẩn luôn mấy cái List con
            if (dataGridView.Columns["ChiTietPhanCongs"] != null) dataGridView.Columns["ChiTietPhanCongs"].Visible = false;
            if (dataGridView.Columns["TienDoCongViecs"] != null) dataGridView.Columns["TienDoCongViecs"].Visible = false;
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "MaLoaiCV" && e.Value != null)
            {
                // Thay vì hiện số ID, ta hiện tên loại
                var id = (int)e.Value;
                var item = context.Loai_Cong_Viec.Find(id);
                if (item != null) e.Value = item.TenLoai;
            }

            if (dataGridView.Columns[e.ColumnIndex].Name == "MaDiaDiem" && e.Value != null)
            {
                // Thay vì hiện số ID, ta hiện tên loại
                var id = (int)e.Value;
                var item = context.Dia_Diem.Find(id);
                if (item != null) e.Value = item.TenDiaDiem;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            xuLyThem = true;
            BatTatChucNang(true);

            // Xóa trắng để nhập mới
            txtTieuDe.Clear();
            rtbNoiDung.Clear();
            dtpBatDau.Value = DateTime.Now;
            dtpKetThuc.Value = DateTime.Now.AddHours(1); // Mặc định họp 1 tiếng
            cboLoaiCongViec.SelectedIndex = 0;
            cboDiaDiem.SelectedIndex = 0;

            txtTieuDe.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;
            xuLyThem = false;
            BatTatChucNang(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. VALIDATION CƠ BẢN
            if (string.IsNullOrWhiteSpace(txtTieuDe.Text))
            {
                MessageBox.Show("Tiêu đề không được để trống!");
                return;
            }

            // 2. VALIDATION NGHIỆP VỤ (Thời gian)
            if (dtpKetThuc.Value <= dtpBatDau.Value)
            {
                MessageBox.Show("Thời gian kết thúc phải lớn hơn thời gian bắt đầu!");
                return;
            }

            // 3. VALIDATION NGHIỆP VỤ (Trùng phòng)
            int diaDiemId = (int)cboDiaDiem.SelectedValue;
            var lichTrung = context.Lich_Lam_Viec.FirstOrDefault(l =>
                l.MaDiaDiem == diaDiemId &&
                l.MaLich != (xuLyThem ? 0 : (int)dataGridView.CurrentRow.Cells["MaLich"].Value) && // Bỏ qua chính nó nếu đang sửa
                ((l.ThoiGianBatDau >= dtpBatDau.Value && l.ThoiGianBatDau < dtpKetThuc.Value) || // Trùng đầu
                 (l.ThoiGianKetThuc > dtpBatDau.Value && l.ThoiGianKetThuc <= dtpKetThuc.Value) || // Trùng đuôi
                 (l.ThoiGianBatDau <= dtpBatDau.Value && l.ThoiGianKetThuc >= dtpKetThuc.Value)) // Trùng lọt lòng
            );

            if (lichTrung != null)
            {
                MessageBox.Show($"Phòng này đang kẹt lịch: {lichTrung.TieuDe}\nTừ: {lichTrung.ThoiGianBatDau:HH:mm} - {lichTrung.ThoiGianKetThuc:HH:mm}");
                return;
            }

            try
            {
                if (xuLyThem)
                {
                    // Thêm Mới
                    Lich_Lam_Viec lich = new Lich_Lam_Viec();
                    lich.TieuDe = txtTieuDe.Text;
                    lich.NoiDung = rtbNoiDung.Text;
                    lich.ThoiGianBatDau = dtpBatDau.Value;
                    lich.ThoiGianKetThuc = dtpKetThuc.Value;
                    lich.MaLoaiCV = (int)cboLoaiCongViec.SelectedValue;
                    lich.MaDiaDiem = (int)cboDiaDiem.SelectedValue;

                    context.Lich_Lam_Viec.Add(lich);
                    context.SaveChanges();
                    MessageBox.Show("Thêm lịch thành công!");
                }
                else
                {
                    // Cập Nhật
                    int id = Convert.ToInt32(dataGridView.CurrentRow.Cells["MaLich"].Value);
                    Lich_Lam_Viec lich = context.Lich_Lam_Viec.Find(id);
                    if (lich != null)
                    {
                        lich.TieuDe = txtTieuDe.Text;
                        lich.NoiDung = rtbNoiDung.Text;
                        lich.ThoiGianBatDau = dtpBatDau.Value;
                        lich.ThoiGianKetThuc = dtpKetThuc.Value;
                        lich.MaLoaiCV = (int)cboLoaiCongViec.SelectedValue;
                        lich.MaDiaDiem = (int)cboDiaDiem.SelectedValue;

                        context.Lich_Lam_Viec.Update(lich);
                        context.SaveChanges();
                        MessageBox.Show("Cập nhật thành công!");
                    }
                }

                LoadData(); // Load lại lưới
                BatTatChucNang(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow == null) return;

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa lịch này?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    int id = Convert.ToInt32(dataGridView.CurrentRow.Cells["MaLich"].Value);
                    Lich_Lam_Viec lich = context.Lich_Lam_Viec.Find(id);
                    if (lich != null)
                    {
                        context.Lich_Lam_Viec.Remove(lich);
                        context.SaveChanges();
                        LoadData();
                        MessageBox.Show("Đã xóa!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể xóa (Có thể do lịch này đã có nhân viên hoặc báo cáo đi kèm).\nLỗi: " + ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            BatTatChucNang(false);
            LoadData(); // Reset lại dữ liệu ban đầu
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
