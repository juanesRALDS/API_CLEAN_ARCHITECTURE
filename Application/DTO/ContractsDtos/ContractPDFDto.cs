using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SagaAserhi.Application.DTO.ContractsDtos;

public class ContractPdfDto
{
    public string ContractNumber { get; set; } = "R-GCL005 No. 3";
    public string ContractType { get; set; } = "CONTRATO DE SERVICIO DE GESTIÓN EXTERNA DE RESIDUOS PELIGROSOS HOSPITALARIOS Y SIMILARES";

    // Datos fijos de ASERHI
    public string ContractorName { get; set; } = "ASERHI S.A.S. E.S.P.";
    public string ContractorNit { get; set; } = "830.502.145-5";
    public string ContractorLegalRep { get; set; } = "YHON ELKIN GIRALDO ARISTIZABAL";
    public string ContractorAddress { get; set; } = "CALLE 16N No.7-69 BARRIO EL RECUERDO, POPAYÁN";
    public string ContractorPhone { get; set; } = "3148908132";
    public string ContractorEmail { get; set; } = "comercial.aserhi@hotmail.com";

    // Datos del cliente
    public string ClientName { get; set; } = String.Empty;
    public string ClientNit { get; set; } = String.Empty;
    public string ClientLegalRep { get; set; } = String.Empty;
    public string ClientAddress { get; set; } = String.Empty;
    public string ClientCity { get; set; } = String.Empty;
    public string ClientDepartment { get; set; } = String.Empty;
    public string ClientPhone { get; set; } = String.Empty;
    public string ClientEmail { get; set; } = String.Empty;

    // Fechas
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Datos de residuos y tarifas
    public List<WasteService> WasteServices { get; set; } = new();
}

public class WasteService
{
    public string SiteName { get; set; } = string.Empty;
    public string Municipality { get; set; } = string.Empty;
    public string WasteType { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}