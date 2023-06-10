export class User {
    id?: number;
    firstname?: string;
    lastname?: string;
    username?: string;
    email?: string;
    password?: string;
    address?: string;
    provider?: string;
    roleId?:number;
    idtoken?: string
    token?: string;
}

export class TokenResponse {
    userName?: string;
    token?: string;
}