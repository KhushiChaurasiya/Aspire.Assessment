import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertService } from '../_services/alert.service';
import { HttpClient } from '@angular/common/http';
import { first } from 'rxjs';
import { App } from '../_models/app';

@Component({
  selector: 'app-add-edit',
  templateUrl: './add-edit.component.html'
})
export class AddEditComponent implements OnInit {
  form!: FormGroup;
  id?: any;
  title!: string;
  loading = false;
  submitting = false;
  submitted = false;
  selectedFile!: File;
  warning: boolean;
  appDetails: any = [];
  fileName = '';
  numRegex = /^-?\d*[.,]?\d{0,2}$/;
  filetype : string;

  constructor(
      private formBuilder: FormBuilder,
      private route: ActivatedRoute,
      private router: Router,
      private userService: UserService,
      private alertService: AlertService,
      private http:HttpClient
  ) { }

  ngOnInit() {
      this.id = this.route.snapshot.params['id'];
      this.form = this.formBuilder.group({
          name: ['', Validators.required],
          description: ['', Validators.required],
          price: ['', [Validators.required, Validators.pattern(this.numRegex)]],
          type: ['', Validators.required],
          files: ['', Validators.required],
      });

      this.title = 'Add App';
      if (this.id) {
          // edit mode
          this.title = 'Edit App';
          this.loading = true;
          this.userService.getById(this.id)
              .pipe(first())
              .subscribe({
                next:(x) => {
                 this.form.patchValue(x);
                 this.appDetails = x;
                 this.form.get('files')!.setValue(this.appDetails.files);
                 this.fileName = this.appDetails.files;
                  this.loading = false;
                },
                error: (error: any) => {
                  this.alertService.error(error);
                  this.submitting = false;
              }
              });
      }
  }

  // convenience getter for easy access to form fields
  get f() { return this.form.controls; }

  onSubmit() {
      this.submitted = true;

      // reset alerts on submit
      this.alertService.clear();

      // stop here if form is invalid
      if (this.form.invalid) {
          return;
      }

      this.submitting = true;
      if(this.filetype != "application/x-zip-compressed")
      {
         this.alertService.error("Unsupported file format, upload only zip");
         this.submitting = false;
      }else{
        this.saveApp()
          .subscribe({
              next: () => {
                  this.alertService.success('App detail saved', { keepAfterRouteChange: true });
                  this.router.navigateByUrl('/devDash/appsdetails');
              },
              error: (error: any) => {
                  this.alertService.error(error);
                  this.submitting = false;
              }
          })
        }
  }

  onSelectFile(fileInput: any) {
  this.fileName = "";
   this.selectedFile = fileInput.target.files[0];
      this.fileName = this.selectedFile.name;
        // this.form.get('files')!.setValue(this.selectedFile);
        this.filetype = this.selectedFile.type;
        if(this.filetype != "application/x-zip-compressed")
        {
          this.alertService.error("Unsupported file format, upload only zip");
        }


    //   if(this.MaxSizeValidation(this.selectedFile.size) == true)
    //   {
    //     // return;
    //   }
  }

  private saveApp() {
    debugger;
      const filedata = new FormData();
      if(this.id != 0 && this.id != undefined){
      filedata.append('id',this.id);
      
        // return false;
        
      if(this.selectedFile == null && this.selectedFile == undefined && this.selectedFile == "")
      {
          filedata.append('filedetails', this.appDetails.files);
      }
      }else{
          filedata.append('filesdetails', this.selectedFile , this.selectedFile.name);
      }
      filedata.append('name',this.form.value.name);
      filedata.append('description',this.form.value.description);
      filedata.append('price',this.form.value.price);
      filedata.append('type',this.form.value.type);
      filedata.append('files',this.form.value.files);
      return this.id
          ? this.userService.put(this.id, filedata)
          : this.userService.post(filedata);
    

  }
  private MaxSizeValidation(size :number)
  {
    const allowed_type = ['application/x-zip-compressed']
    if (size >= 2097152 * 1024) {
        this.warning = true;  
    }else{
         this.alertService.error("File size max then 2Mb");
         return this.warning =false;
    }
    return this.warning;
  }
}
