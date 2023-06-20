import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './_helpers/auth.guard';


const accountModule = () => import('./account/account.module').then(x => x.AccountModule);
const userDashModule = () => import('./userDashboard/user.module').then(x => x.UserModule);
const adminDashModule = () => import('./adminDashboard/admin-dash.module').then(x => x.AdminDashModule);
const devDashModule =() => import('./developerDashboard/devdash.module').then(x=>x.DevdashModule)
const routes: Routes = [
    // { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'account', loadChildren: accountModule },
    { path: 'userDash', loadChildren: userDashModule, canActivate: [AuthGuard]},
    { path: 'adminDash', loadChildren:adminDashModule, canActivate: [AuthGuard]},
    { path: 'devDash', loadChildren:devDashModule, canActivate: [AuthGuard] },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }