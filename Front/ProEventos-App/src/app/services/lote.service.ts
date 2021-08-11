import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Lote } from '@app/models/Lote';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoteService {
  baseURL = 'https://localhost:5001/api/v1/lotes';

  constructor(private http: HttpClient) { }

  getLotesByEventoId(eventoId: number): Observable<Lote[]> {
    return this.http.get<Lote[]>(`${this.baseURL}/${eventoId}`);
  }


  saveLote(eventoId: number, lotes: Lote[]): Observable<Lote[]> {
    return this.http.put<Lote[]>(`${this.baseURL}/${eventoId}`, lotes);
  }

  deleteLote(eventoId: number, loteId: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/${eventoId}/${loteId}`);
  }

}
