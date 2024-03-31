import { Cidade } from "./cidade.model";

export interface RegiaoCidade {
    id: string;
    regiaoId: string;
    cidadeId: string;
    cidade: Cidade;
}

export interface Regiao {
    id?: string;
    nome: string;
    ativo: boolean;
    cidadesId: string[];
    regiaoCidade?: { cidadeId: string }[];
  }
