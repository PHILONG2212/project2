using System;
using System.Collections.Generic;

namespace project2.Entities;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public string MaKh { get; set; } = null!;

    public DateTime NgayDat { get; set; }

    public DateTime NgayCan { get; set; }

    public DateTime NgayGiao { get; set; }

    public string? HoTen { get; set; }

    public string DiaChi { get; set; } = null!;

    public string CachThanhToan { get; set; } = null!;

    public string CachVanChuyen { get; set; } = null!;

    public double PhiVanChuyen { get; set; }

    public int TrangThai { get; set; }

    public string? MaVn { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual NhanVien? MaVnNavigation { get; set; }
}
