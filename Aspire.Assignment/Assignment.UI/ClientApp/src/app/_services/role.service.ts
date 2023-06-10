import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from "src/environments/environment";
import { Role } from "../_models/role";

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient) { }
    public getAll() {
      return this.http.get<Role[]>(`${environment.apiUrl}/api/Role`);
    }
}
