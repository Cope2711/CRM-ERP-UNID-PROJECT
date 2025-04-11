import React from 'react';

const About: React.FC = () => {
  return (
    <section className="bg-white py-16 px-6 md:px-24">
      <div className="max-w-6xl mx-auto flex flex-col md:flex-row items-center gap-12">
        {/* Imagen */}
        <div className="flex-1">
          <img
            src="/minecraft-1.svg"
            alt="Logo ERP"
            className="rounded-xl shadow-lg w-full max-w-[80px] md:max-w-[100px] mx-auto object-contain"
          />
        </div>

        {/* Contenido */}
        <div className="flex-1 text-gray-800">
          <h2 className="text-4xl font-bold mb-4">¿Quiénes Somos?</h2>
          <p className="text-lg text-gray-600 mb-4">
            Ayudamos a empresas a construir marcas increíbles y productos superiores.
            Nuestra perspectiva es ofrecer servicios digitales atractivos y eficientes.
          </p>
          <p className="text-sm text-gray-500 mb-6">
            Somos una empresa de software especializada en ERP (Enterprise Resource Planning), brindando
            herramientas modernas, seguras y escalables para la transformación digital de las empresas.
          </p>

          <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
            <div>
              <h3 className="font-semibold text-lg mb-1">Marca Versátil</h3>
              <p className="text-sm text-gray-500">
                Creamos soluciones digitales que funcionan en todos los medios y dispositivos.
              </p>
            </div>

            <div>
              <h3 className="font-semibold text-lg mb-1">Agencia Digital</h3>
              <p className="text-sm text-gray-500">
                Creemos en la innovación integrando ideas elaboradas y tecnología.
              </p>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default About;