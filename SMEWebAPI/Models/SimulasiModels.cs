using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMEWebAPI.Models
{
    public class SimulasiInput
    {
        [Required]
        public int CalcType { get; set; }
        // 1 = Berdasarkan Plafond
        // 2 = Berdasarkan Angsuran

        [Required]
        public double Amount { get; set; }
        // CalcType = 1 --> Amount = Plafond
        // CalcType = 2 --> Amount = Angsuran

        [Required]
        public int Tenor { get; set; }
        // Dalam Bulan

        [Required]
        public double Rate { get; set; }
        // Dalam Persen Per Tahun

        [Required]
        public int RateType { get; set; }
        // 1 = Flat
        // 2 = Anuitas
    }

    public class SimulasiRow
    {
        public int BulanKe { get; set; }
        public double AngsuranPokok { get; set; }
        public double AngsuranBunga { get; set; }
        public double TotalAngsuran { get; set; }
        public double SisaPinjaman { get; set; }

        public SimulasiRow() { }

        public SimulasiRow(int bulanKe, double angsuranPokok, double angsuranBunga, double totalAngsuran, double sisaPinjaman)
        {
            this.BulanKe = bulanKe;
            this.AngsuranPokok = angsuranPokok;
            this.AngsuranBunga = angsuranBunga;
            this.TotalAngsuran = totalAngsuran;
            this.SisaPinjaman = sisaPinjaman;
        }
    }

    public class SimulasiOutput
    {
        public double Plafond { get; set; }
        public double AngsuranPokok { get; set; }
        public double AngsuranBunga { get; set; }
        public double TotalAngsuran { get; set; }
        public IEnumerable<SimulasiRow> TabelAngsuran { get; set; }

        public SimulasiOutput() { }

        public SimulasiOutput(double plafond, 
            double angsuranPokok, double angsuranBunga, double totalAngsuran, IEnumerable<SimulasiRow> tabelAngsuran)
        {
            this.Plafond = plafond;
            this.AngsuranPokok = angsuranPokok;
            this.AngsuranBunga = angsuranBunga;
            this.TotalAngsuran = totalAngsuran;
            this.TabelAngsuran = tabelAngsuran;
        }
    }
}