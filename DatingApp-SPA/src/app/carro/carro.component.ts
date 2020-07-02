import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-carro',
  templateUrl: './carro.component.html',
  styleUrls: ['./carro.component.css'],
})
export class CarroComponent implements OnInit {
  lista: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getLista();
  }

  getLista() {
    this.http.get('http://localhost:5000/api/carro').subscribe(
      (response) => {
        this.lista = response;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
