using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.VisualBasic;
using SMEWebAPI.Models;

namespace SMEWebAPI.Controllers
{
    public class SimulasiController : ApiController
    {
        // POST: api/Simulasi
        public IHttpActionResult Post([FromBody] SimulasiInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            /*
            List<SimulasiRow> tabel = new List<SimulasiRow> {
                new SimulasiRow(1, 100, 10, 120, 500),
                new SimulasiRow(2, 100, 10, 120, 400),
                new SimulasiRow(3, 100, 10, 120, 300)
            };
            SimulasiOutput output = new SimulasiOutput(500, 100, 10, 110, tabel);
            */

            // Calculation

            double limit = 0;
            int tenor = 0;
            double rate = 0;
            double angspokok = 0;
            double angsbunga = 0;
            double angsuran = 0;

            SimulasiOutput output = new SimulasiOutput();

            if (input.CalcType == 1) // Berdasarkan Plafon
            {
                try { limit = input.Amount; }
                catch { limit = 0; }
                try { rate = input.Rate / 100.0; }
                catch { rate = 0; }
                try { tenor = input.Tenor; }
                catch { tenor = 0; }

                try
                {
                    if (input.RateType == 1) // Flat
                    {
                        angspokok = limit / tenor;

                        angsbunga = (limit * (rate / 12));

                        angsuran = angspokok + angsbunga;

                        output.Plafond = limit;
                        output.AngsuranPokok = angspokok;
                        output.AngsuranBunga = angsbunga;
                        output.TotalAngsuran = angsuran;
                    }
                    else if (input.RateType == 2) // Anuitas
                    {
                        double dangsuran;
                        dangsuran = Financial.Pmt(rate / 12, tenor, -limit);

                        angsuran = float.Parse(dangsuran.ToString());

                        output.Plafond = limit;
                        output.AngsuranPokok = Financial.PPmt(rate / 12, 1, tenor, -limit);
                        output.AngsuranBunga = Financial.IPmt(rate / 12, 1, tenor, -limit);
                        output.TotalAngsuran = angsuran;
                    }
                    else
                    {
                        angsuran = 0;

                        output.Plafond = limit;
                        output.AngsuranPokok = 0;
                        output.AngsuranBunga = 0;
                        output.TotalAngsuran = angsuran;
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            else if (input.CalcType == 2) // Berdasarkan Angsuran
            {
                try { angsuran = input.Amount; }
                catch { angsuran = 0; }
                try { rate = input.Rate / 100.0; }
                catch { rate = 0; }
                try { tenor = input.Tenor; }
                catch { tenor = 0; }

                try
                {
                    if (input.RateType == 1) // Flat
                    {
                        limit = ((1 - ((rate / 12) * tenor)) / 1) * angsuran * tenor;

                        output.Plafond = limit;
                        output.AngsuranPokok = limit / tenor;
                        output.AngsuranBunga = limit * (rate / 12);
                        output.TotalAngsuran = angsuran;
                    }
                    else if (input.RateType == 2) // Anuitas
                    {
                        double dlimit;
                        dlimit = Financial.PV(rate / 12, tenor, -angsuran);

                        limit = float.Parse(dlimit.ToString());

                        output.Plafond = limit;
                        output.AngsuranPokok = Financial.PPmt(rate / 12, 1, tenor, -limit);
                        output.AngsuranBunga = Financial.IPmt(rate / 12, 1, tenor, -limit);
                        output.TotalAngsuran = angsuran;
                    }
                    else
                    {
                        limit = 0;

                        output.Plafond = limit;
                        output.AngsuranPokok = 0;
                        output.AngsuranBunga = 0;
                        output.TotalAngsuran = angsuran;
                    }
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
            else // Berdasarkan Error
            {
                return BadRequest("Berdasarkan Tidak Valid!");
            }

            // Tabel Angsuran

            List<SimulasiRow> simulasitabel = new List<SimulasiRow>();

            double jumlangspokok = 0, jumlangsbunga = 0, jumltotangsuran = 0;
            DataTable dt_cicilan;
            dt_cicilan = new DataTable();

            dt_cicilan.Rows.Add();
            dt_cicilan.Columns.Add("BULANKE");
            dt_cicilan.Columns.Add("ANGSURANPOKOK");
            dt_cicilan.Columns.Add("ANGSURANBUNGA");
            dt_cicilan.Columns.Add("TOTALANGSURAN");
            dt_cicilan.Columns.Add("SISAPINJAMAN");

            jumlangspokok = 0;
            jumlangsbunga = 0;
            jumltotangsuran = 0;

            double iapokok, iabunga, iatotal, ipsisa;

            iapokok = 0;
            iabunga = 0;
            iatotal = iapokok + iabunga;
            ipsisa = limit;

            jumlangspokok = jumlangspokok + iapokok;
            jumlangsbunga = jumlangsbunga + iabunga;
            jumltotangsuran = jumltotangsuran + iatotal;

            dt_cicilan.Rows[0]["BULANKE"] = 0;
            dt_cicilan.Rows[0]["ANGSURANPOKOK"] = iapokok;
            dt_cicilan.Rows[0]["ANGSURANBUNGA"] = iabunga;
            dt_cicilan.Rows[0]["TOTALANGSURAN"] = iatotal;
            dt_cicilan.Rows[0]["SISAPINJAMAN"] = ipsisa;

            simulasitabel.Add(new SimulasiRow(0, iapokok, iabunga, iatotal, ipsisa));

            for (int i = 1; i <= tenor; i++)
            {
                if (input.RateType == 1) // Flat
                {
                    iapokok = limit / tenor;
                    if (iapokok > ipsisa)
                        iapokok = ipsisa;
                    iabunga = limit * (rate / 12);
                    iatotal = iapokok + iabunga;
                    ipsisa = ipsisa - iapokok;
                    if (ipsisa < 0)
                        ipsisa = 0.0;
                }
                else if (input.RateType == 2) // Anuitas
                {
                    iapokok = Financial.PPmt(rate / 12, i, tenor, -limit);
                    if (iapokok > ipsisa)
                        iapokok = ipsisa;
                    iabunga = Financial.IPmt(rate / 12, i, tenor, -limit);
                    iatotal = iapokok + iabunga;
                    ipsisa = ipsisa - iapokok;
                    if (ipsisa < 0)
                        ipsisa = 0.0;
                }
                else
                {
                    iapokok = 0;
                    iabunga = 0;
                    iatotal = 0;
                    ipsisa = 0;
                }

                jumlangspokok = jumlangspokok + iapokok;
                jumlangsbunga = jumlangsbunga + iabunga;
                jumltotangsuran = jumltotangsuran + iatotal;

                dt_cicilan.Rows.Add();
                dt_cicilan.Rows[i]["BULANKE"] = i;
                dt_cicilan.Rows[i]["ANGSURANPOKOK"] = iapokok;
                dt_cicilan.Rows[i]["ANGSURANBUNGA"] = iabunga;
                dt_cicilan.Rows[i]["TOTALANGSURAN"] = iatotal;
                dt_cicilan.Rows[i]["SISAPINJAMAN"] = ipsisa;

                simulasitabel.Add(new SimulasiRow(i, iapokok, iabunga, iatotal, ipsisa));
            }

            // Footer Jumlah
            // simulasitabel.Add(new SimulasiRow(999, jumlangspokok, jumlangsbunga, jumltotangsuran, 0));

            output.TabelAngsuran = simulasitabel;

            return Ok(output);
        }
    }
}
