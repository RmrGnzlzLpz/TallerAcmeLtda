using NUnit.Framework;
using Domain.Entities;
using System;
using System.Linq;

namespace Domain.Test
{
    public class CreditoTests
    {
        Empleado empleado;
        Credito credito;
        [SetUp]
        public void Setup()
        {
            /* En los escenario de prueba se mencion� "Valor a Pagar", dichos valores deben tomarse como el valor solicitado y no como el saldo total */
            empleado = new Empleado
            {
                Cedula = "1082470166",
                Nombre = "Ramiro Gonz�lez",
                Salario = 1200000
            };
        }

        /*
         * HU 001 - Como usuario quiero registrar cr�ditos por libranzas para llevar el control de cr�ditos
         * Criterios:
         *  1. Los cr�ditos deben ser por valores entre 5 mill�n y 10 millones
         *  2. El valor del inter�s del cr�dito ser� de 0.5% por cada mes de plazo
         *  3. El plazo para el pago del cr�dito debe ser de m�ximo 10 meses
         *  4. El valor total para pagar ser� valor del cr�dito + Tasa Inter�s por el plazo del cr�dito SaldoInicialCredito = ValorCredito*(1 + TasaInteres x PlazoCredito)
         *  5. El sistema debe llevar el registro de las cuotas
         * **/
        [Test]
        public void ValorInferiorIncorrecto()
        {
            Exception ex = Assert.Throws<Exception>(() => empleado.SolicitarCredito(valor: 4000000, plazo: 10));
            Assert.AreEqual("El valor del cr�dito debe estar entre 5 y 10 millones.", ex.Message);
        }
        
        [Test]
        public void ValorSuperiorIncorrecto()
        {
            Exception ex = Assert.Throws<Exception>(() => empleado.SolicitarCredito(15000000, 10));
            Assert.AreEqual("El valor del cr�dito debe estar entre 5 y 10 millones.", ex.Message);
        }

        [Test]
        public void PlazoIncorrecto()
        {
            Exception ex = Assert.Throws<Exception>(() => empleado.SolicitarCredito(6000000, 11));
            Assert.AreEqual("El plazo para el pago del cr�dito debe ser de m�ximo 10 meses.", ex.Message);
        }

        [Test]
        public void SolicitudCorrecta()
        {
            string response = empleado.SolicitarCredito(6000000, 10);
            Assert.AreEqual("Cr�dito registrado. Valor a pagar: $6300000.", response);
        }

        /*
         * HU 002 - Como cliente Quiero registrar abonos de dinero al cr�dito para ir amortizando el valor de dicho cr�dito
         *  1. El cliente puede abonar m�nimo el valor de la cuota pendiente, pero puede decir abonar m�s del valor correspondiente lo cual se descontar�a de las cuotas siguientes
         *  2. El sistema debe llevar el registro de los abonos
         *  3. Los abonos a los cr�ditos deben ser mayor a 0 y no pueden superar el saldo del cr�dito.
         *  4. El valor abonado del cr�dito se debe mantener registrado en el sistema para futuras consultas
         * **/
        [Test]
        public void AbonoMenorACuota()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del cr�dito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            Exception ex = Assert.Throws<Exception>(() => credito.Abonar(500000));
            Assert.AreEqual("El valor del abono debe ser m�nimo de $630000.", ex.Message);
        }

        [Test]
        public void AbonoCeroONegativo()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del cr�dito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            Exception ex = Assert.Throws<Exception>(() => credito.Abonar(0));
            Assert.AreEqual("El valor del abono es incorrecto.", ex.Message);
        }

        [Test]
        public void AbonoSuperiorASaldo()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del cr�dito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            Exception ex = Assert.Throws<Exception>(() => credito.Abonar(7000000));
            Assert.AreEqual("El valor del abono es incorrecto.", ex.Message);
        }

        [Test]
        public void AbonoCorrecto()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del cr�dito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(630000);
            credito.Abonar(630000);
            string response = credito.Abonar(800000);
            Assert.AreEqual("Abono registrado correctamente. Su nuevo saldo es: $4240000.", response);
        }

        /*
         * HU 003 - Como cliente quiero consultar el saldo de cada cuota del cr�dito para conocer el estado de su cr�dito.
         *  1. El sistema debe visualizar el listado de abonos realizados a un cr�dito, visualizando su valor y fecha de abono.
         */
        [Test]
        public void ConsultarCuotas()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del cr�dito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(1890000);
            Assert.AreEqual($"Numero = 2, Estado = Pagada, Valor = 630000, Saldo = 0, Fecha = {credito.Cuotas[1].FechaDePago}", credito.Cuotas[1].ToString());
        }


        /*
         *  HU 004 - Como cliente quiero consulta la lista de los abonos realizados a mi cr�dito para conocer el hist�rico de pagos.
         *   1. El sistema debe visualizar el listado de abonos realizados a un cr�dito, visualizando su valor y fecha de abono
         */
        [Test]
        public void ConsultarAbonos()
        {
            // En el escenario de prueba, los 6 millones aparecen como "Valor a pagar", pero debe tomarse como el valor inicial del cr�dito.
            CreateCredit(6000000, 10);
            credito = empleado.Creditos.FirstOrDefault();
            credito.Abonar(660000);
            credito.Abonar(820000);
            credito.Abonar(700000);
            var abono = credito.Abonos[2];
            Assert.AreEqual($"Valor = 700000, Fecha = {abono.FechaDeCreacion}", abono.ToString());
        }

        [Test]
        public void ListarAbonos()
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