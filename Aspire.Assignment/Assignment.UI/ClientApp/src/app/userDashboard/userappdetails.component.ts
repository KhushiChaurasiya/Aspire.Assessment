import { Component, OnInit } from '@angular/core';
import { App } from '../_models/app';
import { UserService } from '../_services/user.service';
import { HttpEventType, HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-userappdetails',
  templateUrl: './userappdetails.component.html'
})
export class UserappdetailsComponent implements OnInit {
  apps: App[];
  searchTerm = '';
  constructor(private userService: UserService) { }

  ngOnInit() {
  this.getAllApp();
  }

  public download(fileInput: any) {
    this.userService.downloadFile(fileInput.files).subscribe(async (event) => {
        let data = event as HttpResponse < Blob > ;
        const downloadedFile = new Blob([data.body as BlobPart], {
            type: data.body?.type
        });
        if (downloadedFile.type != "") {
            const a = document.createElement('a');
            a.setAttribute('style', 'display:none;');
            document.body.appendChild(a);
            a.download = fileInput.files;
            a.href = URL.createObjectURL(downloadedFile);
            a.target = '_blank';
            a.click();
            document.body.removeChild(a);
            this.userService.DownloadCount(fileInput.id).subscribe(data=>{
              console.log(data);
            });
        }
    });


  }

  search(value: string): void {    
    if(value == ""){
      this.getAllApp();
    }
    else{
      this.apps = this.apps.filter((val) =>
      val.name?.includes(value));
    }
  }

  getAllApp()
  {
    this.userService.getAll()
    // .pipe(first())
     .subscribe(apps => this.apps = apps);
  }

}
