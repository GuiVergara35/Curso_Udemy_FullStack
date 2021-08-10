import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ActivatedRoute } from '@angular/router';
import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modoSalvar = 'post';
  evento = {} as Evento;
  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }
  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService) {
    this.localeService.use('pt-br')
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

    if (eventoIdParam !== null) {
      this.spinner.show();

      this.modoSalvar = 'put';
      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
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

  ngOnInit() {
    this.carregarEvento();
    this.validation();
  }
  public validation(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemUrl: ['', Validators.required]
    });
  }

  resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public salvarAlteracao(): void {
    this.spinner.show();
    if (this.form.valid) {

      if (this.modoSalvar === 'post') {
        this.evento = { ...this.form.value };
        this.eventoService[this.modoSalvar](this.evento).subscribe(
          () => this.toastr.success('Evento salvo com sucesso', 'Sucesso!'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Algo deu errado', 'Erro')
          },
          () => this.spinner.hide()
        );

      } else {
        this.evento = { id: this.evento.id, ...this.form.value };
        this.eventoService['put'](this.evento).subscribe(
          () => this.toastr.success('Evento alterado com sucesso', 'Sucesso!'),
          (error: any) => {
            console.error(error);
            this.spinner.hide();
            this.toastr.error('Algo deu errado', 'Erro')
          },
          () => this.spinner.hide()
        );
      }
    }
  }
}
