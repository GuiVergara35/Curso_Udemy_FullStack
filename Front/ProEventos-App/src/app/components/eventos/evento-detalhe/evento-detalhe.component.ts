import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ActivatedRoute, Router } from '@angular/router';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { Evento } from '@app/models/Evento';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Lote } from '@app/models/Lote';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit
{
  modalRef: BsModalRef;
  eventoId: number;
  modoSalvar = 'post';
  evento = {} as Evento;
  form!: FormGroup;
  loteAtual = {id: 0, nome: '', indice:0};

  get modoEditar(): boolean
  {
    return this.modoSalvar === 'put';
  }

  get lotes(): FormArray
  {
    return this.form.get('lotes') as FormArray
  }

  get f(): any
  {
    return this.form.controls;
  }

  get bsConfig(): any
  {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }

  get bsConfigLote(): any
  {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }
  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService,
    private loteService: LoteService)
    {
    this.localeService.use('pt-br')
    }

  public carregarEvento(): void
  {
    this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.eventoId !== null && this.eventoId !== 0)
    {
      this.spinner.show();

      this.modoSalvar = 'put';
      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
          this.evento.lote.forEach(lot => this.lotes.push(this.criarLote(lot)));
        },
        error: (any) => {
          this.spinner.hide();
          console.error(Error);
          this.toastr.error('Erro ao tentar carregar evento', 'Erro!');
        },
        complete: () => {
          this.spinner.hide();
        }
      });
    }
  }

  ngOnInit()
  {
    this.carregarEvento();
    this.validation();
  }
  public validation(): void
  {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', Validators.required],
      lotes: this.fb.array([])
    });
  }

  adicionarLote(): void
  {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  criarLote(lote: Lote): FormGroup
  {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
      quantidade: [, Validators.required],
    });
  }

  resetForm(): void
  {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any
  {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public salvarAlteracao(): void
  {
    this.spinner.show();
    if (this.form.valid)
    {

      this.evento = (this.modoSalvar === 'post')
        ? {...this.form.value}
        : { id: this.evento.id, ...this.form.value};

        this.eventoService[this.modoSalvar](this.evento).subscribe(
          (eventoRetorno: Evento) => {

            this.toastr.success('Evento salvo com sucesso', 'Sucesso!');
            this.router.navigate([`eventos/detalhes/${eventoRetorno.id}`]);
          },
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Algo deu errado', 'Erro')
          },
          () => this.spinner.hide()
        );
    }
  }

  public salvarLote(): void {
    this.spinner.show();
    if(this.form.controls.lotes.valid){
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success('Lote salvos com sucesso', 'Sucesso!');
          this.lotes.reset();
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar lotes', 'Erro!');
          console.error(error);
        },
        () => {},
      ).add(() => this.spinner.hide())
    }
  }

  public removerLote(template: TemplateRef<any>,
                      indice: number): void {

    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class:'modeal-sm'})

  }

  confirmDeleteLote(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso', 'Sucesso');
        this.lotes.removeAt(this.loteAtual.indice)
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar deletar o lote', 'Erro')
      },
    ).add(() => this.spinner.hide());
  }

  declineDeleteLote(): void {
    this.modalRef.hide();
  }

}


