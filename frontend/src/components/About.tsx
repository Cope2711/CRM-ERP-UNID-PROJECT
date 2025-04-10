import React from 'react';

const About: React.FC = () => {
  return (
    <section className="bg-white py-12 px-6 md:px-16 lg:px-32 text-gray-800">
      <div className="max-w-4xl mx-auto text-center">
        <h1 className="text-4xl font-bold mb-6 text-blue-600">Sobre Nosotros</h1>
        <p className="text-lg mb-4">
          Somos una empresa de software dedicada a ofrecer soluciones tecnológicas que optimizan la gestión empresarial.
        </p>
        <p className="text-lg mb-4">
          Nuestro producto principal es un <span className="font-semibold text-gray-900">ERP (Enterprise Resource Planning)</span> completo, diseñado para integrar todas las áreas de tu negocio en una sola plataforma eficiente y fácil de usar.
        </p>
        <p className="text-lg">
          Nos enfocamos en brindar herramientas modernas, escalables y seguras para que nuestros clientes puedan centrarse en hacer crecer sus negocios.
        </p>
      </div>
    </section>
  );
};

export default About;