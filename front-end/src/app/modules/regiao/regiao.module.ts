import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegiaoComponent } from './regiao.component';
import { RegiaoRoutingModule } from './regiao.routing';
import { RegiaoCadastroComponent } from './regiao-cadastro/regiao-cadastro.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RegiaoRoutingModule
  ],
  declarations: [
    RegiaoComponent,
    RegiaoCadastroComponent
  ],
  exports: [
    RegiaoComponent
  ]
})
export class RegiaoModule { }
