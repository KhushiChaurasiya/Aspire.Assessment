import { HttpClient, HttpEvent, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { App } from "../_models/app";
import { Observable } from "rxjs";


@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private http: HttpClient) { }

    public getAll() {
        return this.http.get<App[]>(`${environment.apiUrl}/api/App`);
    }

    public getAllUser() {
        debugger;
        return this.http.get<App[]>(`${environment.apiUrl}/api/User`);
    }

    public getById(id: number) {
        return this.http.get<object>(`${environment.apiUrl}/api/App/${id}`);
    }

    public post(app: any) : Observable<any> {
        return this.http.post<any>(`${environment.apiUrl}/api/App`, app);
    }

    public put(id: number,app: any) {
        debugger;
        return this.http.put<any>(`${environment.apiUrl}/api/App?id=${id}`,app);
    }

    public delete(id: number) {
        return this.http.delete<App[]>(`${environment.apiUrl}/api/App/${id}`);
    }

    // public downloadFile(file: string) {
    //     return this.http.get(`${environment.apiUrl}/api/App/download/${file}`);
    //   }

      public downloadFile(file: string): Observable<HttpEvent<Blob>> {
        return this.http.request(new HttpRequest(
          'GET',
          `${environment.apiUrl}/api/App?file=${file}`,
          null,
          {
            reportProgress: true,
            responseType: 'blob'
          }));
      }

      public DownloadCount(AppId : any)
      {
        debugger;
        return this.http.put<any>(`${environment.apiUrl}/api/App/AppDownload?AppId=${AppId}`,null);
      }

      public getDownloadedReport() {
        return this.http.get<any>(`${environment.apiUrl}/api/App/DownloadedReport`);
      }

      public getAllUserCountReport(){
        return this.http.get<any>(`${environment.apiUrl}/api/User/GetAllUserCountReport`)
      }
    public getAllLogReport() {
      return this.http.get<any>(`${environment.apiUrl}/api/App/LogReport`);
  }
} 