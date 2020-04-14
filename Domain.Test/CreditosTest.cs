using NUnit.Framework;
using Domain.Entities;
using System;
using System.Linq;
using System.Collections;

namespace Domain.Test
{
    public class CreditosTests
    {
        Empleado empleado;
        Credito credito;
        [SetUp]
        public void Setup()
        {
            /* En los escenario de prueba se mencionó "Valor a Pagar", dichos valores deben tomarse como el valor solicitado y no como el saldo total */
            empleado = new Empleado
            {
                Cedula = "1082470166",
                Nombre = "Ramiro González",
                Salario = 1200000
            };
        }

        /*
         * HU 001 - Como usuario quiero registrar créditos por libranzas para llevar el control de créditos
         * Criterios:
         *  1. Los créditos deben ser por valores entre 5 millón y 10 millones
         *  2. El valor del interés del crédito será de 0.5% por cada mes de plazo
         *  3. El plazo para el pago del crédito debe ser de máximo 10 meses
         *  4. El valor total para pagar será valor del crédito + Tasa Interés por el plazo del crédito SaldoInicialCredito = ValorCredito*(1 + TasaInteres x PlazoCredito)
         *  5. El sistema debe llevar el registro de las cuotas
         * **/
        private static IEnumerable SIncorrectas()
        {
            yield return new TestCaseData(4000000, 10, "El valor del crédito debe estar entre 5 y 10 millones.").SetName("ValorInferiorIncorrecto");
            yield return new TestCaseData(15000000, 10, "El valor del crédito debe estar entre 5 y 10 millones.").SetName("ValorSuperiorIncorrecto");
            yield return new TestCaseData(6000000, 11, "El plazo para el pago del crédito debe ser de máximo 10 meses.").SetName("PlazoIncorrecto");
        }

        [TestCaseSource("SIncorrectas")]
        public void SolicitudesIncorrectas(double valor, int plazo, string expected)
        {
            Exception ex = Assert.Throws<Exception>(() => empleado.SolicitarCredito(valor: valor, plazo: plazo));
            Assert.AreEqual(expected, ex.Message);
        }

        [Test]
        public void SolicitudCorrecta()
        {
            string response = empleado.SolicitarCredito(6000000, 10);
            Assert.AreEqual("Crédito registrado. Valor a pagar: $6300000.", response);
        }

        /*
         * HU 002 - Como cliente Quiero registrar abonos de dinero al crédito para ir amortizando el valor de dicho crédito
         *  1. El cliente puede abonar mínimo el valor de la cuota pendiente, pero puede decir abonar más del valor correspondiente lo cual se descontaría de las cuotas siguientes
         *  2. El sistema debe llevar el registro de los abonos
         *  3. Los abonos a los créditos deben ser mayor a 0 y no pueden superar el saldo del crédito.
         *  4. El valor abonado del crédito se debe mantener registrado en el sistema para futuras consultas
         * **/
        [TestCaseSource("AIncorrectos")]
        public void AbonosIncorrectos(double valor, int plazo, double abono, string expected)
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del crédito.
            CreateCredit(valor, plazo);
            credito = empleado.Creditos.FirstOrDefault();
            Exception ex = Assert.Throws<Exception>(() => credito.Abonar(abono));
            Assert.AreEqual(expected, ex.Message);
        }
        private static IEnumerable AIncorrectos()
        {
            yield return new TestCaseData(6000000, 10, 500000, "El valor del abono debe ser mínimo de $630000.").SetName("AbonoMenorCuota");
            yield return new TestCaseData(6000000, 10, 0, "El valor del abono es incorrecto.").SetName("AbonoCeroONegatvo");
            yield return new TestCaseData(6000000, 10, 7000000, "El valor del abono es incorrecto.").SetName("AbonoSuperiorASaldo");
        }

        [Test]
        public void AbonoCorrecto()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del crédito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(630000);
            credito.Abonar(630000);
            string response = credito.Abonar(800000);
            Assert.AreEqual("Abono registrado correctamente. Su nuevo saldo es: $4240000.", response);
        }

        /*
         * HU 003 - Como cliente quiero consultar el saldo de cada cuota del crédito para conocer el estado de su crédito.
         *  1. El sistema debe visualizar el listado de abonos realizados a un crédito, visualizando su valor y fecha de abono.
         */
        [Test]
        public void ConsultarCuotas()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del crédito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(1890000);
            Assert.AreEqual($"Numero = 2, Estado = Pagada, Valor = 630000, Saldo = 0, Fecha = {credito.Cuotas[1].FechaDePago}", credito.Cuotas[1].ToString());
        }


        /*
         *  HU 004 - Como cliente quiero consulta la lista de los abonos realizados a mi crédito para conocer el histórico de pagos.
         *   1. El sistema debe visualizar el listado de abonos realizados a un crédito, visualizando su valor y fecha de abono
         */
        [Test]
        public void ConsultarAbonos()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del crédito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(660000);
            credito.Abonar(820000);
            credito.Abonar(700000);
            var abono = credito.Abonos[2];
            Assert.AreEqual($"Valor = 700000, Fecha = {abono.FechaDeCreacion}", abono.ToString());
        }

        [Test]
        public void ListarAbonosYCuotas()
        {
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(660000);
            credito.Abonar(820000);
            credito.Abonar(700000);
            Assert.AreEqual(3, credito.Abonos.Count);
            Assert.AreEqual(10, credito.Cuotas.Count);
        }

        /* NoTests */
        public void CreateCredit(double amount, int plazo)
        {
            empleado.SolicitarCredito(amount, plazo);
        }
    }
}