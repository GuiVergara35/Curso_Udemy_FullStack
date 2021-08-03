import { HttpClient } from '@angular/common/http';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {
  modalRef!: BsModalRef;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];

  widthImg: number = 150;
  marginImg: number =3;
  showImg: boolean = true;
  private _filterList: string = '';

public get filterList(): string {
  return this._filterList;
}

public set filterList(value: string){
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

  constructor(private eventoService: EventoService,
       private modalService: BsModalService,
       private toastr : ToastrService,
       private spinner: NgxSpinnerService) { }

  public ngOnInit(): void {
    this.getEventos();
    this.spinner.show();

  }

  public hideImg(){
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

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {

    this.modalRef.hide();
    this.toastr.success('Deletado com sucesso!', 'Deletado')
  }

  decline(): void {

    this.modalRef.hide();
  }
}
function next(next: any, arg1: (eventos: Evento[]) => void, error: any, arg3: (error: any) => (error: any) => void) {
  throw new Error('Function not implemented.');
}

