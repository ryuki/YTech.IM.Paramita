﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YTech.IM.Paramita.Enums
{
    public enum EnumReports
    {
        [StringValue("Daftar Master Merek")]
        RptBrand,
        [StringValue("Daftar Master Cost Center")]
        RptCostCenter,
        [StringValue("Laporan Jurnal")]
        RptJournal,
        [StringValue("Laporan Neraca")]
        RptNeraca,
        [StringValue("Laporan Laba / Rugi")]
        RptLR,
        [StringValue("Laporan Neraca Gabungan")]
        RptNeracaSum,
        [StringValue("Laporan Laba / Rugi Gabungan")]
        RptLRSum,
        [StringValue("Laporan Kartu Stok")]
        RptStockCard,
        [StringValue("Laporan Stok Per Gudang")]
        RptStockItem,
        [StringValue("Laporan Analisa Budget")]
        RptAnalyzeBudgetDetail,
        [StringValue("Laporan Detail {0}")]
        RptTransDetail,
        [StringValue("Laporan Master Produk")]
        RptItem,
        [StringValue("Laporan Buku Besar")]
        RptBukuBesar,
        [StringValue("Laporan Jurnal Per Cost Center")]
        RptJournalByCostCenter,
        [StringValue("Laporan Buku Besar Per Cost Center")]
        RptBukuBesarByCostCenter,
        [StringValue("Cetak Bukti Kas")]
        RptPrintCash,
        [StringValue("Cetak Kwitansi")]
        RptPrintKwitansi,
        [StringValue("Cetak Bukti Jurnal Umum")]
        RptPrintGL,
        [StringValue("Laporan Rekap {0} {1}")]
        RptTransRecap
    }
}
