import { Router } from '@angular/router';
import { Component, OnInit, ViewChild} from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable } from 'rxjs';
import { RegiaoService } from '../../services/regiao.service';
import { Regiao } from '../../model/regiao.model';

@Component({
  selector: 'app-regiao',
  templateUrl: './regiao.component.html',
  styleUrls: ['./regiao.component.scss']
})
export class RegiaoComponent implements OnInit {
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  regioes$: Observable<Regiao[]>;
  colunasExibidas = ['nome', 'status', 'acoes'];
  dataSource: MatTableDataSource<Regiao>;

  constructor(private regiaoService: RegiaoService, private router: Router) {}

  ngOnInit() {
    this.loadRegioes();
  }

  editarRegiao(regiao: Regiao) {
    this.router.navigate(['/regiao/editar', regiao.id]);
  }

  visualizarRegiao(regiao: Regiao) {
    this.router.navigate(['/regiao/visualizar', regiao.id]);
  }

  navegarParaCadastro() {
    this.router.navigate(['/regiao/cadastrar'])
  }

  loadRegioes() {
    this.regioes$ = this.regiaoService.getRegioes();
    this.regioes$.subscribe((data) => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.sort = this.sort;
    });
  } 

  toggleAtivo(regiao: Regiao) {
    this.regiaoService.toggleAtivo(regiao).subscribe({
        next: (response) => {
            this.dataSource.data = this.dataSource.data.map(r => {
                if (r.id === regiao.id) {
                    return { ...r, ativo: !r.ativo };
                }
                return r;
            });
            this.dataSource._updateChangeSubscription(); 
        },
        error: (error) => {
            console.error('Erro ao alterar o status da região:', error);
        }
    });
}


   

}
