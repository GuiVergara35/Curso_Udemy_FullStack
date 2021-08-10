import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/evento.service';


@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {
  modalRef!: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public eventoId = 0;

  widthImg: number = 150;
  marginImg: number = 3;
  showImg: boolean = true;
  private _filterList: string = '';

  public get filterList(): string {
    return this._filterList;
  }

  public set filterList(value: string) {
    this._filterList = value;
    this.eventosFiltrados = this.filterList ? this.filterEvents(this.filterList) : this.eventos;
  }

  filterEvents(filterBy: string): Evento[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filterBy) !== -1 ||
        evento.local.toLocaleLowerCase().indexOf(filterBy) !== -1
    );
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.getEventos();
    this.spinner.show();

  }

  public hideImg() {
    this.showImg = !this.showImg;
  }

  public getEventos(): void {

    this.eventoService.getEvento().subscribe({
      next: (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Erro!')
      },
      complete: () => this.spinner.hide()
    });
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {

    this.modalRef.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        console.log(result)
        this.toastr.success('Deletado com sucesso!', 'Deletado');
        this.spinner.hide();
        this.getEventos();

      },
      (error: any) => {
        console.error(Error);
        this.toastr.error('Erro ao tentar deletar o evento', 'Erro!');
        this.spinner.hide();
      },
      () => {

      },
    )

  }

  decline(): void {

    this.modalRef.hide();
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhes/${id}`]);
  }

}
