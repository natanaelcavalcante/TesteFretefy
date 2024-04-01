import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { RegiaoService } from '../../services/regiao.service';
import { Regiao } from '../../model/regiao.model';

@Component({
  selector: 'app-regiao',
  templateUrl: './regiao.component.html',
  styleUrls: ['./regiao.component.scss']
})
export class RegiaoComponent implements OnInit {
  regioes$: Observable<Regiao[]>;
  sortedColumn: string = '';
  sortOrder: 'asc' | 'desc' = 'asc';

  constructor(private regiaoService: RegiaoService, private router: Router) {}

  ngOnInit() {
    this.loadRegioes();
  }

  navegarParaCadastro() {
    this.router.navigate(['/regiao/cadastrar']);
  }

  editarRegiao(regiao: Regiao) {
    this.router.navigate(['/regiao/editar', regiao.id]);
  }

  toggleAtivo(regiao: Regiao) {
    this.regiaoService.toggleAtivo(regiao).subscribe({
      next: () => this.loadRegioes(),
      error: (error) => console.error('Erro ao alterar o status da região:', error)
    });
  }

  loadRegioes() {
    this.regioes$ = this.regiaoService.getRegioes()
      .pipe(
        map(regioes => this.sort(regioes, this.sortedColumn, this.sortOrder))
      );
  }

  onColumnHeaderClick(columnName: string) {
    if (this.sortedColumn === columnName) {
      this.sortOrder = this.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortedColumn = columnName;
      this.sortOrder = 'asc';
    }
    this.loadRegioes();
  }

  sort(regioes: Regiao[], column: string, order: 'asc' | 'desc'): Regiao[] {
    if (!column) return regioes; 
    return [...regioes].sort((a, b) => {
      const valA = a[column];
      const valB = b[column];

      if (valA < valB) {
        return order === 'asc' ? -1 : 1;
      }
      if (valA > valB) {
        return order === 'asc' ? 1 : -1;
      }
      return 0;
    });
  }
}
