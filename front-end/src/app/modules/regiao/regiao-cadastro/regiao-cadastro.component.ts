import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RegiaoService } from '../../../services/regiao.service';
import { CidadeService } from '../../../services/cidade.service';
import { Regiao } from '../../../model/regiao.model';

@Component({
  selector: 'app-regiao-cadastro',
  templateUrl: './regiao-cadastro.component.html',
  styleUrls: ['./regiao-cadastro.component.scss']
})
export class RegiaoCadastroComponent implements OnInit {
  regiaoForm: FormGroup;
  cidadesList: any[] = [];
  isEditMode: boolean = false;
  regiaoId: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private regiaoService: RegiaoService,
    private cidadeService: CidadeService
  ) {
    this.regiaoForm = this.fb.group({
      nome: ['', Validators.required],
      cidades: this.fb.array([], Validators.required)
    });
  }

  ngOnInit(): void {
    this.carregarCidades();
    this.route.paramMap.subscribe(params => {
        const id = params.get('id');
        if (id) {
            this.isEditMode = true;
            this.regiaoId = id;
            this.carregarRegiaoParaEdicao(id);
        }
    });
  }

  carregarCidades() {
    
    this.cidadeService.getCidades().subscribe(
      data => this.cidadesList = data,
      error => console.error(error)
    );
  }

  cidades(): FormArray {
    return this.regiaoForm.get('cidades') as FormArray;
  }

  novaCidade(): FormGroup {
    return this.fb.group({
      cidadeId: ['', Validators.required] 
    });
  }

  adicionarCidade() {
    this.cidades().push(this.novaCidade());
  }

  removerCidade(index: number) {
    this.cidades().removeAt(index);
  }

  carregarRegiaoParaEdicao(id: string) {
    this.regiaoService.getRegiaoById(id).subscribe(
      (regiao: Regiao) => {
        if (regiao) {
          this.regiaoForm.patchValue({
            nome: regiao.nome,
            ativo: regiao.ativo
          });
  
          const cidadeFormGroups = regiao.regiaoCidade.map(rc => this.fb.group({
            cidadeId: rc.cidadeId
          }));
          const cidadeFormArray = this.fb.array(cidadeFormGroups);
          this.regiaoForm.setControl('cidades', cidadeFormArray);
        } else {
          console.error('Dados da região não encontrados');
        }
      },
      error => console.error(error)
    );
  }
  
  onSubmit(event: Event) {
    event.preventDefault();
    if (this.regiaoForm.valid) {
        const formValue = this.regiaoForm.value;
  
        let regiaoData: any = { 
            nome: formValue.nome,
            ativo: true,
            cidadesId: formValue.cidades.map(c => c.cidadeId),
            id: this.regiaoId
        };
  
        if (this.isEditMode) {
            this.updateRegiao(regiaoData);
        } else {
            this.createRegiao(regiaoData);
        }
    } else {
        console.error('Formulário inválido');
    }
  }   

  createRegiao(regiao: Regiao) {
    this.regiaoService.createRegiao(regiao).subscribe({
        next: () => {
            console.log('Região criada com sucesso.');
            this.router.navigate(['/regiao']);
        },
        error: (error) => {
            console.error('Erro ao criar a região:', error);
        }
    });
  }

  updateRegiao(regiao: Regiao) {
    console.log('Updating regiao:', regiao);
    this.regiaoService.updateRegiao(regiao).subscribe({
      next: (response) => {
        console.log('Região atualizada:', response);
        this.router.navigate(['/regiao']); 
      },
      error: (error) => {
        console.error('Erro ao atualizar região:', error);
      }
    });
  }

  onCancel() {
    this.router.navigate(['/regiao']);
  }
}
