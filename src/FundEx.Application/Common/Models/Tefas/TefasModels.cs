namespace FundEx.Application.Common.Models.Tefas;

public class TefasBaseResponse<T>
{
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public List<T> ResultList { get; set; } = [];
    public int? ToplamSayi { get; set; }
    public int? ToplamSayfa { get; set; }
}

public class FonTurResult
{
    public string SfonTurAciklama { get; set; } = null!;
    public int SfonTuru { get; set; }
}

public class FonUnvanResult
{
    public string Tanim { get; set; } = null!;
}

public class FonGrupResult
{
    public int FonGrubu { get; set; }
    public string Fongrupaciklama { get; set; } = null!;
}

public class FonDetayResult
{
    public string FonTipi { get; set; } = null!;
    public string FonTurKod { get; set; } = null!;
    public string FonTurAciklama { get; set; } = null!;
}

public class FonKurucuResult
{
    public string KurucuKodu { get; set; } = null!;
    public string KurucuUnvan { get; set; } = null!;
    public string FonTipi { get; set; } = null!;
}

public class FonGnlBlgResult
{
    public string FonKodu { get; set; } = null!;
    public string FonUnvan { get; set; } = null!;
    public string Tarih { get; set; } = null!;
    public decimal Fiyat { get; set; }
    public long TedPaySayisi { get; set; }
    public int KisiSayisi { get; set; }
    public decimal PortfoyBuyukluk { get; set; }
    public int? Rn { get; set; }
    public decimal? BorsaBultenFiyat { get; set; }
}

public class FonGnlBlgRequest
{
    public string FonTipi { get; set; } = null!;
    public string? FonKodu { get; set; }
    public string? AramaMetni { get; set; }
    public string? FonTurKod { get; set; }
    public string? FonGrubu { get; set; }
    public string? SfonTurKod { get; set; }
    public string BasTarih { get; set; } = null!;
    public string BitTarih { get; set; } = null!;
    public int BasSira { get; set; } = 1;
    public int BitSira { get; set; } = 25;
    public string? FonTurAciklama { get; set; }
    public string Dil { get; set; } = "TR";
    public string? KurucuKod { get; set; }
}

public class FonGnlBlgResponse : TefasBaseResponse<FonGnlBlgResult> { }

public class DagilimResult
{
    public string FonKodu { get; set; } = null!;
    public string FonUnvan { get; set; } = null!;
    public string Tarih { get; set; } = null!;
    public decimal? Bb { get; set; }
    public decimal? Byf { get; set; }
    public decimal? D { get; set; }
    public decimal? Db { get; set; }
    public decimal? Bpp { get; set; }
    public decimal? Btaa { get; set; }
    public decimal? Btas { get; set; }
    public decimal? Dt { get; set; }
    public decimal? Dot { get; set; }
    public decimal? Eut { get; set; }
    public decimal? Fb { get; set; }
    public decimal? Fkb { get; set; }
    public decimal? Gas { get; set; }
    public decimal? Gsykb { get; set; }
    public decimal? Gsyy { get; set; }
    public decimal? Gykb { get; set; }
    public decimal? Gyy { get; set; }
    public decimal? Hb { get; set; }
    public decimal? Hs { get; set; }
    public decimal? Kba { get; set; }
    public decimal? Kh { get; set; }
    public decimal? Khau { get; set; }
    public decimal? Khd { get; set; }
    public decimal? Khtl { get; set; }
    public decimal? Kks { get; set; }
    public decimal? Kksd { get; set; }
    public decimal? Kkstl { get; set; }
    public decimal? Kksyd { get; set; }
    public decimal? Km { get; set; }
    public decimal? Kmbyf { get; set; }
    public decimal? Kmkba { get; set; }
    public decimal? Kmkks { get; set; }
    public decimal? Kibd { get; set; }
    public decimal? Osks { get; set; }
    public decimal? Ost { get; set; }
    public decimal? R { get; set; }
    public decimal? T { get; set; }
    public decimal? Tpp { get; set; }
    public decimal? Tr { get; set; }
    public decimal? Vdm { get; set; }
    public decimal? Vm { get; set; }
    public decimal? Vmau { get; set; }
    public decimal? Vmd { get; set; }
    public decimal? Vmtl { get; set; }
    public decimal? Vint { get; set; }
    public decimal? Yba { get; set; }
    public decimal? Ybkb { get; set; }
    public decimal? Ybosb { get; set; }
    public decimal? Ybyf { get; set; }
    public decimal? Yhs { get; set; }
    public decimal? Ymk { get; set; }
    public decimal? Yyf { get; set; }
    public decimal? Oksyd { get; set; }
    public decimal? Osdb { get; set; }
    public string? BilFiyat { get; set; }
}

public class DagilimRequest
{
    public string FonTipi { get; set; } = null!;
    public string? FonKodu { get; set; }
    public string? AramaMetni { get; set; }
    public string? FonTurKod { get; set; }
    public string? FonGrubu { get; set; }
    public string? SfonTurKod { get; set; }
    public string BasTarih { get; set; } = null!;
    public string BitTarih { get; set; } = null!;
    public int BasSira { get; set; } = 1;
    public int BitSira { get; set; } = 25;
    public string? FonTurAciklama { get; set; }
    public string Dil { get; set; } = "TR";
    public string? KurucuKod { get; set; }
    public string? SFonTurKod { get; set; }
    public string? FonKod { get; set; }
    public string? FonGrup { get; set; }
    public string? FonUnvanTip { get; set; }
}

public class DagilimResponse : TefasBaseResponse<DagilimResult> { }
