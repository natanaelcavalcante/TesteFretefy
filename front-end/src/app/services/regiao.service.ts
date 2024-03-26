import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap  } from 'rxjs';
import { Regiao } from '../model/regiao.model'; 

@Injectable({
  providedIn: 'root'
})
export class RegiaoService {
  private apiUrl = 'http://localhost:5000/api/regiao'; 

  constructor(private http: HttpClient) {}

  getRegioes(): Observable<Regiao[]> {
    return this.http.get<Regiao[]>(this.apiUrl);
  }
  
  toggleAtivo(regiao: Regiao): Observable<any> {
    const url = `${this.apiUrl}/${regiao.id}/toggle-active`;
    return this.http.put(url, {});
}  
  
  updateRegiao(regiao: Regiao): Observable<Regiao> {
    return this.http.put<Regiao>(`${this.apiUrl}/${regiao.id}`, regiao);
  }

  createRegiao(regiao: Regiao): Observable<Regiao> {
    return this.http.post<Regiao>(this.apiUrl, regiao);
  }

  getRegiaoById(id: string): Observable<Regiao> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<Regiao>(url);
  }
  
}
