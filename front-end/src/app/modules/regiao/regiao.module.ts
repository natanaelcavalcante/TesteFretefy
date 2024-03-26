import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSortModule } from '@angular/material/sort';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { RegiaoComponent } from './regiao.component';
import { RegiaoRoutingModule } from './regiao.routing';
import { RegiaoCadastroComponent } from './regiao-cadastro/regiao-cadastro.component';
import { FormsModule, ReactiveFormsModule} from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule,
    MatSortModule, 
    MatFormFieldModule, 
    MatInputModule, 
    MatSelectModule,
    RegiaoRoutingModule,
    ReactiveFormsModule,
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
