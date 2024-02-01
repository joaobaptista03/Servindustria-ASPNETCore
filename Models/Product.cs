namespace Servindustria.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Category Category { get; set; }
    }

    public enum Category
    {
        AcessorioAcoInoxLatãoGalvanizado_Cobre,
        ActuadorPneumáticoEléctrico_Hidráulico,
        Bomba_Electrobomba,
        BorrachaEmFolha_Juntas,
        CartãoParaJuntasDeFlanges,
        Compensador_JuntaDeDilatação,
        ContadorRotámetroCaudalímetroDeÁguaOuVapor,
        ControladorDeTemperaturaPressãoCaudalENível,
        ElectroVálvula,
        EmpanqueDeCordão_Mecânico,
        EquipamentoConfecçãoTêxtil_Geradores_Caldeira_Ferro,
        EquipamentoDeÁguasResiduaisESaneamento,
        EquipamentoAvac,
        EquipamentoDeGás,
        EquipamentoHidráulico,
        EquipamentoContraIncêndio,
        EquipamentoPneumático,
        EquipamentoDePressãoCaudalENível,
        EquipamentoDeSegurança,
        EquipamentoSolar,
        FeltroComprimido_Junta,
        Filtro,
        GatewayDeComunicaçãoParaAutomaçãoIndustrial,
        Indicador_InterruptorMagnéticoDeNível,
        Massa_SprayDeLubrificação,
        PlásticoTécnico,
        Pressostato_Termostato_TransmissorDePressão,
        PurgadorDeVapor,
        SensorElectrónico,
        SondaDeTemperaturaNívelPressãoECaudal,
        TermómetroManómetroHigrómetro,
        VálvulaDeAgulha,
        VálvulaDeCunha,
        VálvulaDeDescargaDaCaldeira,
        VálvulaEléctrica,
        VálvulaDeFoleDuplo,
        VálvulaDeGlobo,
        VálvulaDePistão,
        VálvulaPneumática,
        VálvulaDeReduçãoDePressão,
        VálvulaDeRetenção,
        VálvulaDeSegurança,
        VidroTemperadoBorossilicatoVisorFluxo,
        VidroDeReflexãoTorneiraDeNível
    }
}
