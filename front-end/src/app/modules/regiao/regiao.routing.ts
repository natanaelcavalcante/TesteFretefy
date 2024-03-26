import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegiaoComponent } from './regiao.component';
import { RegiaoCadastroComponent } from './regiao-cadastro/regiao-cadastro.component';

const routes: Routes = [
  { path: '', component: RegiaoComponent },
  { path: 'cadastrar', component: RegiaoCadastroComponent },
  { path: 'editar/:id', component: RegiaoCadastroComponent }, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegiaoRoutingModule { }
